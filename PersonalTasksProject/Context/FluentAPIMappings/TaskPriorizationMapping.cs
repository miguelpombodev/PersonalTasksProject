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
    }
}