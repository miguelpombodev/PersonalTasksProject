using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public abstract class BaseRepository<T>(AppDbContext context) : IBaseRepository<T>
    where T : class
{
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().AsNoTracking().ToListAsync();
    }

    public virtual async void AddAsync(T entity)
    {
        try
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
        }

    }

    public virtual async void UpdateAsync(T entity)
    {
        try
        {
            context.Set<T>().Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
        }
    }

    public virtual async void DeleteAsync(T entity)
    {
        try
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
        }
    }
}