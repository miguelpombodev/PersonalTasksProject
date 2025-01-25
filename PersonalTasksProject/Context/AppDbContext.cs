using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context.FluentAPIMappings;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Context;

public class AppDbContext : DbContext
{
    private readonly IWebHostEnvironment _environment;

    public AppDbContext(DbContextOptions<AppDbContext> options, IWebHostEnvironment environment) : base(options)
    {
        _environment = environment;
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> Tasks { get; set; }
    public DbSet<TaskPriorization> TaskPriorizations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (_environment.IsDevelopment())
        {
            options.LogTo(Console.WriteLine);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserMapping());
        modelBuilder.ApplyConfiguration(new UserTaskMapping());
        modelBuilder.ApplyConfiguration(new TaskPriorizationMapping());
    }
}