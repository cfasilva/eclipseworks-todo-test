using Eclipseworks.Domain.DTOs.ProjectDTO;
using Eclipseworks.Domain.Models;
using Eclipseworks.Infra.Context;
using Eclipseworks.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskStatus = Eclipseworks.Domain.Models.TaskStatus;

namespace Eclipseworks.Service;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectModel>> GetProjectsByUserAsync(int userId)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<ProjectModel> CreateProjectAsync(int userId, ProjectCreationDTO dto)
    {
        var project = new ProjectModel
        {
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return project;
    }

    public async Task DeleteProjectAsync(int userId, int projectId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

        if (project == null)
            throw new InvalidOperationException("Projeto não encontrado.");

        var hasPendingTasks = await _context.Tasks
            .AnyAsync(t => t.ProjectId == projectId && t.Status != TaskStatus.Completed);

        if (hasPendingTasks)
            throw new InvalidOperationException("Não é possível excluir o projeto com tarefas pendentes.");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }
}

