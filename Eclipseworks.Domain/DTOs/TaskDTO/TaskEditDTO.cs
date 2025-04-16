using System.ComponentModel.DataAnnotations;

namespace Eclipseworks.Domain.DTOs.TaskDTO;

public class TaskEditDTO
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100)]
    public string Title { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Due date is required.")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public int Status { get; set; }
}
