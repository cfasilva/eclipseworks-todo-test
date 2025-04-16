using Eclipseworks.Api.Controllers;
using Eclipseworks.Domain.DTOs.ProjectDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Eclipseworks.Test;

public class ProjectControllerTests
{
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly ProjectController _controller;

    public ProjectControllerTests()
    {
        _mockProjectService = new Mock<IProjectService>();
        _controller = new ProjectController(_mockProjectService.Object);
    }

    [Fact]
    public async Task GetProjectsByUser_ShouldReturnOk_WithListOfProjects()
    {
        // Arrange
        var userId = 1;
        var projects = new List<ProjectModel>
        {
            new ProjectModel { Id = 1, Name = "Projeto A", UserId = userId },
            new ProjectModel { Id = 2, Name = "Projeto B", UserId = userId }
        };

        _mockProjectService.Setup(s => s.GetProjectsByUserAsync(userId)).ReturnsAsync(projects);

        // Act
        var result = await _controller.GetProjectsByUser(userId);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ProjectModel>>(ok.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task CreateProject_ShouldReturnCreated_WithProject()
    {
        // Arrange
        var userId = 1;
        var dto = new ProjectCreationDTO
        {
            Name = "Novo Projeto",
            Description = "Projeto teste"
        };

        var project = new ProjectModel
        {
            Id = 10,
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId
        };

        _mockProjectService.Setup(s => s.CreateProjectAsync(userId, dto)).ReturnsAsync(project);

        // Act
        var result = await _controller.CreateProject(userId, dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<ProjectModel>(created.Value);
        Assert.Equal(project.Id, returnValue.Id);
    }

    [Fact]
    public async Task DeleteProject_ShouldReturnNoContent_WhenSuccessful()
    {
        // Arrange
        var userId = 1;
        var projectId = 2;
        _mockProjectService.Setup(s => s.DeleteProjectAsync(userId, projectId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProject(userId, projectId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
