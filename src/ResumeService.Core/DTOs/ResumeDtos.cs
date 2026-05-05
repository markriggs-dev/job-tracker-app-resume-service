namespace ResumeService.Core.DTOs;

public record ResumeResponse(
    Guid Id,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string FileSizeDisplay,
    DateTimeOffset UploadedAt
);

public record JobResumeLinkResponse(
    Guid Id,
    Guid JobRequisitionId,
    Guid ResumeId,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string FileSizeDisplay,
    DateTimeOffset LinkedAt
);

public record LinkResumeToJobRequest(Guid ResumeId);
