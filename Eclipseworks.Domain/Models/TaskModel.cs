using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eclipseworks.Domain.Models;

public class TaskModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public TaskPriority Priority { get; set; }

    public int ProjectId { get; set; }

    [ForeignKey("ProjectId")]
    public ProjectModel? Project { get; set; }

    public ICollection<TaskCommentModel> Comments { get; set; } = new List<TaskCommentModel>();
}

public enum TaskStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}
