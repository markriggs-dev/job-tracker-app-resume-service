using Microsoft.EntityFrameworkCore;
using ResumeService.Core.Interfaces;
using ResumeService.Core.Models;
using ResumeService.Infrastructure.Data;

namespace ResumeService.Infrastructure.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly ResumeServiceDbContext _db;

    public ResumeRepository(ResumeServiceDbContext db) => _db = db;

    public async Task<IEnumerable<ResumeEntry>> GetAllByUserAsync(string userId) =>
        await _db.Resumes
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.UploadedAt)
            .ToListAsync();

    public async Task<ResumeEntry?> GetByIdAsync(Guid id, string userId) =>
        await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

    public async Task<ResumeEntry> CreateAsync(ResumeEntry entry)
    {
        _db.Resumes.Add(entry);
        await _db.SaveChangesAsync();
        return entry;
    }

    public async Task<bool> DeleteAsync(Guid id, string userId)
    {
        var entry = await _db.Resumes.FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);
        if (entry is null) return false;
        _db.Resumes.Remove(entry);
        await _db.SaveChangesAsync();
        return true;
    }
}
