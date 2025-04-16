using Eclipseworks.Api.Controllers;
using Eclipseworks.Domain.DTOs.TaskDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Eclipseworks.Test;

public class TaskControllerTests
{
    private readonly Mock<ITaskService> _mockTaskService;
    private readonly TaskController _controller;

    public TaskControllerTests()
    {
        _mockTaskService = new Mock<ITaskService>();
        _controller = new TaskController(_mockTaskService.Object);
    }

    [Fact]
    public async Task GetTasksByProject_ShouldReturnOk_WithList()
    {
        // Arrange
        var projectId = 1;
        var tasks = new List<TaskModel>
            {
                new TaskModel { Id = 1, Title = "Task 1", ProjectId = projectId },
                new TaskModel { Id = 2, Title = "Task 2", ProjectId = projectId }
            };
        _mockTaskService.Setup(s => s.GetTasksByProjectAsync(projectId)).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetTasksByProject(projectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<TaskModel>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task CreateTask_ShouldReturnCreated_WithTask()
    {
        // Arrange
        var projectId = 1;
        var dto = new TaskCreationDTO { Title = "Nova tarefa", DueDate = DateTime.UtcNow.AddDays(2) };
        var createdTask = new TaskModel { Id = 1, Title = dto.Title, ProjectId = projectId };

        _mockTaskService.Setup(s => s.CreateTaskAsync(projectId, dto)).ReturnsAsync(createdTask);

        // Act
        var result = await _controller.CreateTask(projectId, dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returnValue = Assert.IsType<TaskModel>(created.Value);
        Assert.Equal(createdTask.Id, returnValue.Id);
    }

    [Fact]
    public async Task UpdateTask_ShouldReturnOk_WithUpdatedTask()
    {
        // Arrange
        var projectId = 1;
        var taskId = 1;
        var dto = new TaskEditDTO
        {
            Title = "Atualizada",
            Description = "Desc",
            DueDate = DateTime.UtcNow,
            Status = (int)Domain.Models.TaskStatus.InProgress
        };
        var updatedTask = new TaskModel { Id = taskId, Title = dto.Title, ProjectId = projectId };

        _mockTaskService.Setup(s => s.UpdateTaskAsync(projectId, taskId, dto)).ReturnsAsync(updatedTask);

        // Act
        var result = await _controller.UpdateTask(projectId, taskId, dto);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<TaskModel>(ok.Value);
        Assert.Equal(taskId, returnValue.Id);
    }

    [Fact]
    public async Task DeleteTask_ShouldReturnNoContent()
    {
        // Arrange
        var projectId = 1;
        var taskId = 1;
        _mockTaskService.Setup(s => s.DeleteTaskAsync(projectId, taskId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTask(projectId, taskId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
