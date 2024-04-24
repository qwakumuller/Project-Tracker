using Microsoft.EntityFrameworkCore;
using ProjectTracker.Models;
using Task = ProjectTracker.Models.Task;

namespace ProjectTracker.Data;

public class ProjectTrackerContext : DbContext
{
    public ProjectTrackerContext(DbContextOptions<ProjectTrackerContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<UserProject> UserProject { get; set; }
    public DbSet<Status> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Project>().ToTable("Projects");
        modelBuilder.Entity<Task>().ToTable("Tasks");
        modelBuilder.Entity<Status>().ToTable("Statuses");
        modelBuilder.Entity<UserProject>().ToTable("UserProject");
    }
}