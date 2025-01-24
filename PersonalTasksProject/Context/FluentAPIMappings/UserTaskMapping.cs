using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Context.FluentAPIMappings;

public class UserTaskMapping : IEntityTypeConfiguration<UserTask>
{
    public void Configure(EntityTypeBuilder<UserTask> builder)
    {
        builder.ToTable("user_tasks");
        builder.HasKey(x => x.Id);

        builder.Property(task => task.Id).HasColumnType("UUID").IsRequired();
        builder.Property(task => task.Title).HasColumnType("VARCHAR(100)").IsRequired();
        builder.Property(task => task.Description).HasColumnType("VARCHAR(1000)").IsRequired();
        builder.Property(task => task.DueDate).HasColumnType("DATE").IsRequired();
        builder.Property(task => task.TaskPriorizationId).HasColumnType("INT").IsRequired();
        builder.Property(task => task.CompletionDate).HasColumnType("DATE");
        builder.Property(task => task.UserId).HasColumnType("UUID").IsRequired();
        builder.Property(task => task.CreatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime()).IsRequired();
        builder.Property(task => task.UpdatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime()).IsRequired();

        builder.HasIndex(task => task.TaskPriorizationId).HasDatabaseName("IDX_TASK_PRIORIZATION_ID");
        builder.HasIndex(task => task.UserId).HasDatabaseName("IDX_TASK_USER_ID");
        builder.HasIndex(task => task.DueDate).HasDatabaseName("IDX_TASK_DUE_DATE");
        
        builder.HasOne<User>(task => task.User).WithMany(user => user.Tasks).HasForeignKey(task => task.UserId).HasConstraintName("FK_TASKS_USERS_USERS").IsRequired();
        builder.HasOne<TaskPriorization>(task => task.Status).WithMany(priority => priority.Tasks).HasConstraintName("FK_TASKS_USERS_PRIORITIES_TASKS").IsRequired();        
    }
}