using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context;

namespace PersonalTasksProject.Extensions;

public static class MigrationExtensions
{
    public async static void ApplyMigrationsAsync(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        
        using AppDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await dbContext.Database.MigrateAsync();
    }
}