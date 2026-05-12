using Microsoft.EntityFrameworkCore;
using ResumeService.Core.Models;

namespace ResumeService.Infrastructure.Data;

public class ResumeServiceDbContext : DbContext
{
    public ResumeServiceDbContext(DbContextOptions<ResumeServiceDbContext> options) : base(options) { }

    public DbSet<ResumeEntry> Resumes => Set<ResumeEntry>();
    public DbSet<JobResumeLink> JobResumeLinks => Set<JobResumeLink>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResumeEntry>(e =>
        {
            e.ToTable("resumes");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).HasMaxLength(256).IsRequired();
            e.Property(x => x.FileName).HasMaxLength(512).IsRequired();
            e.Property(x => x.StorageKey).HasMaxLength(1024).IsRequired();
            e.Property(x => x.ContentType).HasMaxLength(128).IsRequired();
            e.HasIndex(x => x.UserId);
        });

        modelBuilder.Entity<JobResumeLink>(e =>
        {
            e.ToTable("job_resume_links");
            e.HasKey(x => x.Id);
            e.Property(x => x.UserId).HasMaxLength(256).IsRequired();
            e.Property(x => x.DocumentType).HasConversion<string>().HasMaxLength(32).IsRequired();
            e.HasIndex(x => new { x.JobRequisitionId, x.UserId, x.DocumentType }).IsUnique();
            e.HasOne(x => x.Resume)
                .WithMany()
                .HasForeignKey(x => x.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
