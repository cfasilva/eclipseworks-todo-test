using Eclipseworks.Domain.DTOs.TaskDTO;
using Eclipseworks.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eclipseworks.Api.Controllers;

[ApiController]
[Route("api/projects/{projectId}/tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET: api/projects/{projectId}/tasks
    [HttpGet]
    public async Task<IActionResult> GetTasksByProject(int projectId)
    {
        var tasks = await _taskService.GetTasksByProjectAsync(projectId);
        return Ok(tasks);
    }

    // POST: api/projects/{projectId}/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] TaskCreationDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var task = await _taskService.CreateTaskAsync(projectId, dto);
        return CreatedAtAction(nameof(GetTasksByProject), new { projectId }, task);
    }

    // PUT: api/projects/{projectId}/tasks/{taskId}
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(int projectId, int taskId, [FromBody] TaskEditDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updated = await _taskService.UpdateTaskAsync(projectId, taskId, dto);
        return Ok(updated);
    }

    // DELETE: api/projects/{projectId}/tasks/{taskId}
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(int projectId, int taskId)
    {
        await _taskService.DeleteTaskAsync(projectId, taskId);
        return NoContent();
    }
}
