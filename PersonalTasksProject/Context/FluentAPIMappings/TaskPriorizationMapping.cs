using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Context.FluentAPIMappings;

public class TaskPriorizationMapping : IEntityTypeConfiguration<TaskPriorization>
{
    public void Configure(EntityTypeBuilder<TaskPriorization> builder)
    {
        builder.ToTable("task_priorizations");
        builder.HasKey(priority => priority.Id);
        
        builder.Property(priority => priority.Id).ValueGeneratedOnAdd();
        builder.Property(priority => priority.Name).HasColumnType("VARCHAR(50)").IsRequired();
        builder.Property(priority => priority.CreatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime()).IsRequired();
        builder.Property(priority => priority.UpdatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime()).IsRequired();


        
        builder.HasIndex(priority => priority.Name).IsUnique();

        builder.HasData(
            new TaskPriorization
            {
                Id = 1,
                Name = "Critical"
            },
            new TaskPriorization
            {
                Id = 2,
                Name = "High"
            },
            new TaskPriorization
            {
                Id = 3,
                Name = "Medium"
            },
            new TaskPriorization
            {
                Id = 4,
                Name = "Low"
            },
            new TaskPriorization
            {
                Id = 5,
                Name = "None"
            }
            );
    }
}