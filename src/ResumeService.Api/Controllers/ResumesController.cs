using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeService.Core.Services;

namespace ResumeService.Api.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly ResumeService.Core.Services.ResumeService _service;

    public ResumesController(ResumeService.Core.Services.ResumeService service) => _service = service;

    private string UserId => User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        ?? User.FindFirst("sub")?.Value
        ?? throw new UnauthorizedAccessException("User ID not found in token.");

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resumes = await _service.GetAllAsync(UserId);
        return Ok(resumes);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        var allowedTypes = new[] { "application/pdf", "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { error = "Only PDF, DOC, and DOCX files are accepted." });

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest(new { error = "File size must not exceed 10 MB." });

        var result = await _service.UploadAsync(UserId, file);
        return CreatedAtAction(nameof(GetAll), result);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var result = await _service.DownloadAsync(id, UserId);
        if (result is null) return NotFound();

        return File(result.Value.Stream, result.Value.ContentType,
            result.Value.FileName);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id, UserId);
        return deleted ? NoContent() : NotFound();
    }
}
