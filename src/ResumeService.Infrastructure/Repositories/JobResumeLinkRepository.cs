using Microsoft.EntityFrameworkCore;
using ResumeService.Core.Interfaces;
using ResumeService.Core.Models;
using ResumeService.Infrastructure.Data;

namespace ResumeService.Infrastructure.Repositories;

public class JobResumeLinkRepository : IJobResumeLinkRepository
{
    private readonly ResumeServiceDbContext _db;

    public JobResumeLinkRepository(ResumeServiceDbContext db) => _db = db;

    public async Task<IEnumerable<JobResumeLink>> GetAllByJobAsync(Guid jobRequisitionId, string userId) =>
        await _db.JobResumeLinks
            .Include(l => l.Resume)
            .Where(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId)
            .ToListAsync();

    public async Task<JobResumeLink> UpsertAsync(Guid jobRequisitionId, string userId, Guid resumeId, DocumentType documentType)
    {
        var existing = await _db.JobResumeLinks
            .FirstOrDefaultAsync(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId && l.DocumentType == documentType);

        if (existing is not null)
        {
            existing.ResumeId = resumeId;
            existing.LinkedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            existing = new JobResumeLink
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                JobRequisitionId = jobRequisitionId,
                ResumeId = resumeId,
                DocumentType = documentType,
                LinkedAt = DateTimeOffset.UtcNow
            };
            _db.JobResumeLinks.Add(existing);
        }

        await _db.SaveChangesAsync();

        return await _db.JobResumeLinks
            .Include(l => l.Resume)
            .FirstAsync(l => l.Id == existing.Id);
    }

    public async Task<bool> DeleteByJobAndTypeAsync(Guid jobRequisitionId, string userId, DocumentType documentType)
    {
        var link = await _db.JobResumeLinks
            .FirstOrDefaultAsync(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId && l.DocumentType == documentType);
        if (link is null) return false;
        _db.JobResumeLinks.Remove(link);
        await _db.SaveChangesAsync();
        return true;
    }
}
