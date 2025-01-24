using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalTasksProject.Entities;

namespace PersonalTasksProject.Context.FluentAPIMappings;

public class UserMapping: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Id).HasColumnType("UUID").IsRequired();
        builder.Property(user => user.Name).HasColumnType("VARCHAR(100)").IsRequired();
        builder.Property(user => user.Email).HasColumnType("VARCHAR(100)").IsRequired();
        builder.Property(user => user.Password).HasColumnType("VARCHAR(400)").IsRequired();
        builder.Property(user => user.AvatarUrl).HasColumnType("VARCHAR(500)");
        builder.Property(user => user.CreatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime());
        builder.Property(user => user.UpdatedAt).HasColumnType("TIMESTAMP WITH TIME ZONE").HasDefaultValue(DateTime.Now.ToUniversalTime());
        
        
        builder.HasIndex(user => user.Email).HasDatabaseName("USER_IDX_EMAIL").IsUnique();
    }
}