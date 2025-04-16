using Eclipseworks.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eclipseworks.Infra.Context;

public class AppDbContext : DbContext
{
    public virtual DbSet<TaskModel> Tasks { get; set; }
    public virtual DbSet<ProjectModel> Projects { get; set; }
    public virtual DbSet<UserModel> Users { get; set; }
    public virtual DbSet<TaskCommentModel> TaskComments { get; set; }
    public virtual DbSet<TaskHistoryModel> TaskHistories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserModel>().HasData(
            new UserModel
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@adm.com",
                Role = UserRole.Admin,
            },
            new UserModel
            {
                Id = 2,
                Name = "User",
                Email = "user@usr.com",
                Role = UserRole.User,
            }
        );
    }
}
