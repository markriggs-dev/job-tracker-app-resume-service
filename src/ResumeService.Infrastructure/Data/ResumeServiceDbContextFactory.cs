using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ResumeService.Infrastructure.Data;

public class ResumeServiceDbContextFactory : IDesignTimeDbContextFactory<ResumeServiceDbContext>
{
    public ResumeServiceDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ResumeServiceDbContext>()
            .UseNpgsql("Host=localhost;Database=jobtracker;Username=jobtracker;Password=jobtracker")
            .Options;
        return new ResumeServiceDbContext(options);
    }
}
