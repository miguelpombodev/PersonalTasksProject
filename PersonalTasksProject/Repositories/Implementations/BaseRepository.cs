using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PersonalTasksProject.Context;
using PersonalTasksProject.Repositories.Interfaces;

namespace PersonalTasksProject.Repositories.Implementations;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetByPropertyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public virtual async Task<IEnumerable<T>> GetAllByPropertyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);

            return entity;
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
            throw;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
            throw;
        }
    }

    public virtual async void UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
        }
        catch (DbUpdateException dbUpdateException)
        {
            Console.WriteLine($"Database Error - {dbUpdateException.Message}");
            throw;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
            throw;
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
            throw;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"General Error - {exception.Message}");
            throw;
        }
    }

    // protected async Task<List<T>> PaginateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    // {
    //     var count = source.Count();
    //     var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    //
    //     return items;
    // }
}