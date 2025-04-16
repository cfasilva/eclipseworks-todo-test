using Eclipseworks.Domain.DTOs.ProjectDTO;
using Eclipseworks.Domain.Models;

namespace Eclipseworks.Service.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectModel>> GetProjectsByUserAsync(int userId);
    Task<ProjectModel> CreateProjectAsync(int userId, ProjectCreationDTO dto);
    Task DeleteProjectAsync(int userId, int projectId);
}
