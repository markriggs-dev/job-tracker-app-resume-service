using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeService.Core.DTOs;
using ResumeService.Core.Services;

namespace ResumeService.Api.Controllers;

[ApiController]
[Route("api/jobs/{jobId:guid}/resume")]
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
        var link = await _service.GetJobResumeAsync(jobId, UserId);
        return link is null ? NotFound() : Ok(link);
    }

    [HttpPut]
    public async Task<IActionResult> Link(Guid jobId, [FromBody] LinkResumeToJobRequest request)
    {
        var link = await _service.LinkResumeToJobAsync(jobId, UserId, request.ResumeId);
        return Ok(link);
    }

    [HttpDelete]
    public async Task<IActionResult> Unlink(Guid jobId)
    {
        var deleted = await _service.UnlinkResumeFromJobAsync(jobId, UserId);
        return deleted ? NoContent() : NotFound();
    }
}
