using Eclipseworks.Domain.DTOs.ProjectDTO;
using Eclipseworks.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eclipseworks.Api.Controllers;

[ApiController]
[Route("api/users/{userId}/projects")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // GET: api/users/{userId}/projects
    [HttpGet]
    public async Task<IActionResult> GetProjectsByUser(int userId)
    {
        var projects = await _projectService.GetProjectsByUserAsync(userId);
        return Ok(projects);
    }

    // POST: api/users/{userId}/projects
    [HttpPost]
    public async Task<IActionResult> CreateProject(int userId, [FromBody] ProjectCreationDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var project = await _projectService.CreateProjectAsync(userId, dto);
        return CreatedAtAction(nameof(GetProjectsByUser), new { userId = userId }, project);
    }

    // DELETE: api/users/{userId}/projects/{projectId}
    [HttpDelete("{projectId}")]
    public async Task<IActionResult> DeleteProject(int userId, int projectId)
    {
        await _projectService.DeleteProjectAsync(userId, projectId);
        return NoContent();
    }
}

