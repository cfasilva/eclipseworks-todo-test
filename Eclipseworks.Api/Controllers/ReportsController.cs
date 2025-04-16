using Eclipseworks.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eclipseworks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/reports/avg-tasks-completed
    [HttpGet("avg-tasks-completed")]
    public async Task<IActionResult> GetAverageTasksCompleted([FromHeader(Name = "X-User-Role")] string role)
    {
        if (role?.ToLower() != "admin")
            return Forbid("Apenas usuários com perfil de 'administrador' podem acessar este relatório.");

        var last30Days = DateTime.UtcNow.AddDays(-30);

        var query = await _context.Tasks
            .Where(t => t.Status == Domain.Models.TaskStatus.Completed && t.DueDate >= last30Days)
            .GroupBy(t => t.Project.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                UserName = _context.Users.FirstOrDefault(u => u.Id == g.Key)!.Name,
                TasksCompleted = g.Count(),
                AvgPerDay = g.Count() / 30.0
            })
            .ToListAsync();

        return Ok(query);
    }
}

