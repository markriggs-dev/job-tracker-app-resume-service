using ResumeService.Core.Models;

namespace ResumeService.Core.Interfaces;

public interface IJobResumeLinkRepository
{
    Task<JobResumeLink?> GetByJobAsync(Guid jobRequisitionId, string userId);
    Task<JobResumeLink> UpsertAsync(Guid jobRequisitionId, string userId, Guid resumeId);
    Task<bool> DeleteByJobAsync(Guid jobRequisitionId, string userId);
}
