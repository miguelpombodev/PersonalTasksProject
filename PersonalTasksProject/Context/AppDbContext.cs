using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context.FluentAPIMappings;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Context;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<TaskPriorization> TaskPriorizations { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new UserTaskMapping());
        modelBuilder.ApplyConfiguration(new TaskPriorizationMapping());
    }
}