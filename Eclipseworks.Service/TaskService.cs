using Eclipseworks.Domain.DTOs.TaskDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Infra.Context;
using Eclipseworks.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Eclipseworks.Domain.Models.TaskStatus;

namespace Eclipseworks.Service;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskModel>> GetTasksByProjectAsync(int projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<TaskModel> CreateTaskAsync(int projectId, TaskCreationDTO dto)
    {
        int taskCount = await _context.Tasks
            .CountAsync(t => t.ProjectId == projectId);

        if (taskCount >= 20)
            throw new InvalidOperationException("Limite de 20 tarefas por projeto atingido.");

        var task = new TaskModel
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = projectId,
            Priority = (TaskPriority)dto.Priority
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<TaskModel> UpdateTaskAsync(int projectId, int taskId, TaskEditDTO dto)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

        if (task == null)
            throw new InvalidOperationException("Tarefa não encontrada.");

        if (dto.Title != task.Title)
        {
            var taskHistory = new TaskHistoryModel
            {
                TaskId = task.Id,
                ModifiedAt = DateTime.UtcNow,
                FieldChanged = "Title",
                OldValue = task.Title,
                NewValue = dto.Title,
            };
            _context.TaskHistories.Add(taskHistory);

            task.Title = dto.Title;
        }

        if (dto.Description != task.Description)
        {
            var taskHistoryDescription = new TaskHistoryModel
            {
                TaskId = task.Id,
                ModifiedAt = DateTime.UtcNow,
                FieldChanged = "Description",
                OldValue = task.Description ?? "",
                NewValue = dto.Description ?? "",
            };
            _context.TaskHistories.Add(taskHistoryDescription);

            task.Description = dto.Description;
        }

        if (dto.DueDate != task.DueDate)
        {
            var taskHistory = new TaskHistoryModel
            {
                TaskId = task.Id,
                ModifiedAt = DateTime.UtcNow,
                FieldChanged = "DueDate",
                OldValue = task.DueDate.ToString("o"),
                NewValue = dto.DueDate.ToString("o"),
            };
            _context.TaskHistories.Add(taskHistory);

            task.DueDate = dto.DueDate;
        }

        if (dto.Status != (int)task.Status)
        {
            var taskHistory = new TaskHistoryModel
            {
                TaskId = task.Id,
                ModifiedAt = DateTime.UtcNow,
                FieldChanged = "Status",
                OldValue = task.Status.ToString(),
                NewValue = ((TaskStatus)dto.Status).ToString(),
            };
            _context.TaskHistories.Add(taskHistory);

            task.Status = (TaskStatus)dto.Status;
        }

        await _context.SaveChangesAsync();
        return task;
    }

    public async Task DeleteTaskAsync(int projectId, int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

        if (task == null)
            throw new InvalidOperationException("Tarefa não encontrada.");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}

