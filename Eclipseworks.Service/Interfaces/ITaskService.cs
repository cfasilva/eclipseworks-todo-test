using Eclipseworks.Domain.DTOs.TaskDTO;
using Eclipseworks.Domain.Models;

namespace Eclipseworks.Service.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskModel>> GetTasksByProjectAsync(int projectId);
    Task<TaskModel> CreateTaskAsync(int projectId, TaskCreationDTO dto);
    Task<TaskModel> UpdateTaskAsync(int projectId, int taskId, TaskEditDTO dto);
    Task DeleteTaskAsync(int projectId, int taskId);
}
