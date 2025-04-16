using Eclipseworks.Domain.DTOs.ProjectDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Infra.Context;
using Eclipseworks.Service;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Eclipseworks.Domain.Models.TaskStatus;

namespace Eclipseworks.Test.Services;

public class ProjectServiceTests
{
    private readonly AppDbContext _context;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new ProjectService(_context);
    }

    [Fact]
    public async Task CreateProject_ShouldAddNewProject()
    {
        // Arrange
        int userId = 1;
        var dto = new ProjectCreationDTO
        {
            Name = "Projeto Teste",
            Description = "Descrição do projeto"
        };

        // Act
        var result = await _service.CreateProjectAsync(userId, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(userId, result.UserId);
    }

    [Fact]
    public async Task GetProjectsByUser_ShouldReturnCorrectProjects()
    {
        // Arrange
        int userId = 42;
        _context.Projects.AddRange(
            new ProjectModel { Id = 1, Name = "Projeto 1", UserId = userId },
            new ProjectModel { Id = 2, Name = "Projeto 2", UserId = userId },
            new ProjectModel { Id = 3, Name = "Outro usuário", UserId = 99 }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetProjectsByUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, p => Assert.Equal(userId, p.UserId));
        Assert.Equal(2, ((List<ProjectModel>)result).Count);
    }

    [Fact]
    public async Task DeleteProject_ShouldRemove_WhenAllTasksCompleted()
    {
        // Arrange
        int userId = 1;
        var project = new ProjectModel { Id = 2, Name = "Projeto OK", UserId = userId };
        _context.Projects.Add(project);

        _context.Tasks.Add(new TaskModel
        {
            Id = 2,
            Title = "Tarefa concluída",
            ProjectId = 2,
            Status = TaskStatus.Completed,
            DueDate = DateTime.UtcNow.AddDays(-1),
            Priority = TaskPriority.Medium
        });

        await _context.SaveChangesAsync();

        // Act
        await _service.DeleteProjectAsync(userId, 2);

        // Assert
        var deleted = await _context.Projects.FindAsync(2);
        Assert.Null(deleted);
    }
}
