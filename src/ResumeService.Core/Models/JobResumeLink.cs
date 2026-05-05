namespace ResumeService.Core.Models;

public class JobResumeLink
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid JobRequisitionId { get; set; }
    public Guid ResumeId { get; set; }
    public DateTimeOffset LinkedAt { get; set; }

    public ResumeEntry Resume { get; set; } = null!;
}
