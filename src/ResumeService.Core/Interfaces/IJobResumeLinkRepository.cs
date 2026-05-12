using ResumeService.Core.Models;

namespace ResumeService.Core.Interfaces;

public interface IJobResumeLinkRepository
{
    Task<IEnumerable<JobResumeLink>> GetAllByJobAsync(Guid jobRequisitionId, string userId);
    Task<JobResumeLink> UpsertAsync(Guid jobRequisitionId, string userId, Guid resumeId, DocumentType documentType);
    Task<bool> DeleteByJobAndTypeAsync(Guid jobRequisitionId, string userId, DocumentType documentType);
}
