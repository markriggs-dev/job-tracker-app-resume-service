using ResumeService.Core.Models;

namespace ResumeService.Core.Interfaces;

public interface IResumeRepository
{
    Task<IEnumerable<ResumeEntry>> GetAllByUserAsync(string userId);
    Task<ResumeEntry?> GetByIdAsync(Guid id, string userId);
    Task<ResumeEntry> CreateAsync(ResumeEntry entry);
    Task<bool> DeleteAsync(Guid id, string userId);
}
