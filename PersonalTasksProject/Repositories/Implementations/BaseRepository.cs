using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public abstract class BaseRepository<T>() : IBaseRepository<T>
    where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context) : this()
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async void AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
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
            _dbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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

    public virtual async Task<bool> DeleteByIdAsync(Guid id)
    {
        try
        {
            var rowsAffected = await _dbSet.Where(e => EF.Property<Guid>(e, "Id") == id).ExecuteDeleteAsync();

            return rowsAffected > 0;
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
        }

        return false;
    }
}