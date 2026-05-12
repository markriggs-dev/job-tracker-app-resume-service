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
    string DocumentType,
    string FileName,
    string ContentType,
    long FileSizeBytes,
    string FileSizeDisplay,
    DateTimeOffset LinkedAt
);

public record JobDocumentsResponse(
    JobResumeLinkResponse? Resume,
    JobResumeLinkResponse? CoverLetter
);

public record LinkDocumentToJobRequest(Guid ResumeId, string DocumentType);
