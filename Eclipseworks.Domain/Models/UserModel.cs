using System.ComponentModel.DataAnnotations;

namespace Eclipseworks.Domain.Models;

public class UserModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; } = UserRole.User;

    public ICollection<ProjectModel> Projects { get; set; } = new List<ProjectModel>();
}

public enum UserRole
{
    Admin = 0,
    User = 1
}
