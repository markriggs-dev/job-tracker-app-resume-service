using Microsoft.AspNetCore.Http;
using ResumeService.Core.DTOs;
using ResumeService.Core.Interfaces;
using ResumeService.Core.Models;

namespace ResumeService.Core.Services;

public class ResumeService
{
    private readonly IResumeRepository _resumeRepo;
    private readonly IJobResumeLinkRepository _linkRepo;
    private readonly IStorageService _storage;

    public ResumeService(IResumeRepository resumeRepo, IJobResumeLinkRepository linkRepo, IStorageService storage)
    {
        _resumeRepo = resumeRepo;
        _linkRepo = linkRepo;
        _storage = storage;
    }

    public async Task<IEnumerable<ResumeResponse>> GetAllAsync(string userId)
    {
        var entries = await _resumeRepo.GetAllByUserAsync(userId);
        return entries.Select(MapToResponse);
    }

    public async Task<ResumeResponse> UploadAsync(string userId, IFormFile file)
    {
        var resumeId = Guid.NewGuid();
        var storageKey = $"{userId}/{resumeId}/{file.FileName}";

        using var stream = file.OpenReadStream();
        await _storage.UploadAsync(storageKey, stream, file.Length, file.ContentType);

        var entry = new ResumeEntry
        {
            Id = resumeId,
            UserId = userId,
            FileName = file.FileName,
            StorageKey = storageKey,
            ContentType = file.ContentType,
            FileSizeBytes = file.Length,
            UploadedAt = DateTimeOffset.UtcNow
        };

        var created = await _resumeRepo.CreateAsync(entry);
        return MapToResponse(created);
    }

    public async Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(Guid id, string userId)
    {
        var entry = await _resumeRepo.GetByIdAsync(id, userId);
        if (entry is null) return null;

        var stream = await _storage.DownloadAsync(entry.StorageKey);
        return (stream, entry.FileName, entry.ContentType);
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var entry = await _resumeRepo.GetByIdAsync(id, userId);
        if (entry is null) return false;

        await _storage.DeleteAsync(entry.StorageKey);
        return await _resumeRepo.DeleteAsync(id, userId);
    }

    public async Task<JobResumeLinkResponse?> GetJobResumeAsync(Guid jobRequisitionId, string userId)
    {
        var link = await _linkRepo.GetByJobAsync(jobRequisitionId, userId);
        return link is null ? null : MapLinkToResponse(link);
    }

    public async Task<JobResumeLinkResponse> LinkResumeToJobAsync(Guid jobRequisitionId, string userId, Guid resumeId)
    {
        var link = await _linkRepo.UpsertAsync(jobRequisitionId, userId, resumeId);
        return MapLinkToResponse(link);
    }

    public async Task<bool> UnlinkResumeFromJobAsync(Guid jobRequisitionId, string userId)
    {
        return await _linkRepo.DeleteByJobAsync(jobRequisitionId, userId);
    }

    private static string FormatFileSize(long bytes) => bytes switch
    {
        < 1024 => $"{bytes} B",
        < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
        _ => $"{bytes / (1024.0 * 1024):F1} MB"
    };

    private static ResumeResponse MapToResponse(ResumeEntry e) => new(
        e.Id, e.FileName, e.ContentType, e.FileSizeBytes,
        FormatFileSize(e.FileSizeBytes), e.UploadedAt);

    private static JobResumeLinkResponse MapLinkToResponse(JobResumeLink l) => new(
        l.Id, l.JobRequisitionId, l.ResumeId,
        l.Resume.FileName, l.Resume.ContentType, l.Resume.FileSizeBytes,
        FormatFileSize(l.Resume.FileSizeBytes), l.LinkedAt);
}
