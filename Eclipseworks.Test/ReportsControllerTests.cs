using Eclipseworks.Api.Controllers;
using Eclipseworks.Domain.Models;
using Eclipseworks.Infra.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Eclipseworks.Domain.Models.TaskStatus;

namespace Eclipseworks.Test;

public class ReportsControllerTests
{
    private readonly AppDbContext _context;
    private readonly ReportsController _controller;

    public ReportsControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _controller = new ReportsController(_context);
    }

    [Fact]
    public async Task GetAverageTasksCompleted_ShouldReturn403_WhenUserIsNotAdmin()
    {
        // Act
        var result = await _controller.GetAverageTasksCompleted("user");

        // Assert
        var forbid = Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task GetAverageTasksCompleted_ShouldReturnData_WhenUserIsAdmin()
    {
        // Arrange
        var user1 = new UserModel { Id = 1, Name = "Maria", Email = "maria@empresa.com", Role = UserRole.Admin };
        var user2 = new UserModel { Id = 2, Name = "João", Email = "joao@empresa.com", Role = UserRole.User };

        _context.Users.AddRange(user1, user2);

        var project1 = new ProjectModel { Id = 1, Name = "Projeto A", UserId = 1 };
        var project2 = new ProjectModel { Id = 2, Name = "Projeto B", UserId = 2 };

        _context.Projects.AddRange(project1, project2);

        var now = DateTime.UtcNow;

        _context.Tasks.AddRange(
            new TaskModel { Id = 1, Title = "T1", ProjectId = 1, Status = TaskStatus.Completed, DueDate = now.AddDays(-5), Priority = TaskPriority.Medium },
            new TaskModel { Id = 2, Title = "T2", ProjectId = 1, Status = TaskStatus.Completed, DueDate = now.AddDays(-2), Priority = TaskPriority.Medium },
            new TaskModel { Id = 3, Title = "T3", ProjectId = 2, Status = TaskStatus.Completed, DueDate = now.AddDays(-10), Priority = TaskPriority.Low },
            new TaskModel { Id = 4, Title = "T4", ProjectId = 2, Status = TaskStatus.Pending, DueDate = now.AddDays(3), Priority = TaskPriority.High }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAverageTasksCompleted("admin");

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);

        Assert.NotEmpty(data);
    }
}
