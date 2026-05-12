using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeService.Core.DTOs;
using ResumeService.Core.Models;

namespace ResumeService.Api.Controllers;

[ApiController]
[Route("api/jobs/{jobId:guid}/documents")]
[Authorize]
public class JobResumeLinksController : ControllerBase
{
    private readonly ResumeService.Core.Services.ResumeService _service;

    public JobResumeLinksController(ResumeService.Core.Services.ResumeService service) => _service = service;

    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet]
    public async Task<IActionResult> Get(Guid jobId)
    {
        var docs = await _service.GetJobDocumentsAsync(jobId, UserId);
        return Ok(docs);
    }

    [HttpPut]
    public async Task<IActionResult> Link(Guid jobId, [FromBody] LinkDocumentToJobRequest request)
    {
        if (!Enum.TryParse<DocumentType>(request.DocumentType, out var docType))
            return BadRequest(new { error = "Invalid document type. Must be 'Resume' or 'CoverLetter'." });

        var link = await _service.LinkDocumentToJobAsync(jobId, UserId, request.ResumeId, docType);
        return Ok(link);
    }

    [HttpDelete("{documentType}")]
    public async Task<IActionResult> Unlink(Guid jobId, string documentType)
    {
        if (!Enum.TryParse<DocumentType>(documentType, out var docType))
            return BadRequest(new { error = "Invalid document type. Must be 'Resume' or 'CoverLetter'." });

        var deleted = await _service.UnlinkDocumentFromJobAsync(jobId, UserId, docType);
        return deleted ? NoContent() : NotFound();
    }
}
