using Microsoft.EntityFrameworkCore;
using ResumeService.Core.Interfaces;
using ResumeService.Core.Models;
using ResumeService.Infrastructure.Data;

namespace ResumeService.Infrastructure.Repositories;

public class JobResumeLinkRepository : IJobResumeLinkRepository
{
    private readonly ResumeServiceDbContext _db;

    public JobResumeLinkRepository(ResumeServiceDbContext db) => _db = db;

    public async Task<JobResumeLink?> GetByJobAsync(Guid jobRequisitionId, string userId) =>
        await _db.JobResumeLinks
            .Include(l => l.Resume)
            .FirstOrDefaultAsync(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId);

    public async Task<JobResumeLink> UpsertAsync(Guid jobRequisitionId, string userId, Guid resumeId)
    {
        var existing = await _db.JobResumeLinks
            .FirstOrDefaultAsync(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId);

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
                LinkedAt = DateTimeOffset.UtcNow
            };
            _db.JobResumeLinks.Add(existing);
        }

        await _db.SaveChangesAsync();

        return await _db.JobResumeLinks
            .Include(l => l.Resume)
            .FirstAsync(l => l.Id == existing.Id);
    }

    public async Task<bool> DeleteByJobAsync(Guid jobRequisitionId, string userId)
    {
        var link = await _db.JobResumeLinks
            .FirstOrDefaultAsync(l => l.JobRequisitionId == jobRequisitionId && l.UserId == userId);
        if (link is null) return false;
        _db.JobResumeLinks.Remove(link);
        await _db.SaveChangesAsync();
        return true;
    }
}
