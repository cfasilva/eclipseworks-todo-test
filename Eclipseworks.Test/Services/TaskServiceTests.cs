using Eclipseworks.Domain.DTOs.TaskDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Infra.Context;
using Eclipseworks.Service;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Eclipseworks.Domain.Models.TaskStatus;

namespace Eclipseworks.Test.Services;

public class TaskServiceTests
{
    private readonly AppDbContext _context;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new TaskService(_context);
    }

    [Fact]
    public async Task CreateTask_ShouldAdd_WhenUnderLimit()
    {
        // Arrange
        int projectId = 1;
        _context.Projects.Add(new ProjectModel { Id = projectId, Name = "Projeto", UserId = 1 });

        await _context.SaveChangesAsync();

        var dto = new TaskCreationDTO
        {
            Title = "Nova Tarefa",
            Description = "Detalhes",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = (int)TaskPriority.Medium
        };

        // Act
        var result = await _service.CreateTaskAsync(projectId, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.ProjectId);
    }

    [Fact]
    public async Task CreateTask_ShouldThrow_WhenTaskLimitExceeded()
    {
        // Arrange
        int projectId = 2;
        _context.Projects.Add(new ProjectModel { Id = projectId, Name = "Cheio", UserId = 1 });

        for (int i = 0; i < 20; i++)
        {
            _context.Tasks.Add(new TaskModel
            {
                Title = $"Tarefa {i}",
                ProjectId = projectId,
                DueDate = DateTime.UtcNow,
                Priority = TaskPriority.Low,
                Status = TaskStatus.Pending
            });
        }

        await _context.SaveChangesAsync();

        var dto = new TaskCreationDTO
        {
            Title = "Tarefa extra",
            DueDate = DateTime.UtcNow.AddDays(1),
            Priority = (int)TaskPriority.High
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateTaskAsync(projectId, dto));
    }

    [Fact]
    public async Task UpdateTask_ShouldChangeEditableFields_Only()
    {
        // Arrange
        int projectId = 3;
        _context.Projects.Add(new ProjectModel { Id = projectId, Name = "Projeto", UserId = 1 });

        var task = new TaskModel
        {
            Id = 100,
            Title = "Original",
            Description = "Desc",
            DueDate = DateTime.UtcNow.AddDays(2),
            Status = TaskStatus.Pending,
            Priority = TaskPriority.High,
            ProjectId = projectId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var dto = new TaskEditDTO
        {
            Title = "Atualizada",
            Description = "Nova descrição",
            DueDate = DateTime.UtcNow.AddDays(3),
            Status = (int)TaskStatus.InProgress,
        };

        // Act
        var result = await _service.UpdateTaskAsync(projectId, task.Id, dto);

        // Assert
        Assert.Equal("Atualizada", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority); // prioridade original mantida
        Assert.Equal(TaskStatus.InProgress, result.Status);
    }

    [Fact]
    public async Task UpdateTask_ShouldCreateHistory_WhenFieldsChange()
    {
        // Arrange
        int projectId = 4;
        _context.Projects.Add(new ProjectModel { Id = projectId, Name = "Histórico", UserId = 1 });

        var task = new TaskModel
        {
            Id = 200,
            Title = "Tarefa",
            Description = "Antes",
            DueDate = DateTime.UtcNow,
            Status = TaskStatus.Pending,
            Priority = TaskPriority.Medium,
            ProjectId = projectId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var dto = new TaskEditDTO
        {
            Title = "Tarefa",
            Description = "Depois",
            DueDate = task.DueDate,
            Status = (int)TaskStatus.Completed,
        };

        // Act
        await _service.UpdateTaskAsync(projectId, task.Id, dto);

        var history = await _context.TaskHistories
            .Where(h => h.TaskId == task.Id)
            .ToListAsync();

        // Assert
        Assert.Equal(2, history.Count); // Description + Status
        Assert.Contains(history, h => h.FieldChanged == "Description");
        Assert.Contains(history, h => h.FieldChanged == "Status");
    }
}
