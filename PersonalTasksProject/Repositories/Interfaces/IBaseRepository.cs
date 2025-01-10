using System.Linq.Expressions;

namespace PersonalTasksProject.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllByPropertyAsync(Expression<Func<T, bool>> expression);
    Task<T> AddAsync(T entity);
    void UpdateAsync(T entity);
    Task<bool> DeleteByIdAsync(Guid id);
}