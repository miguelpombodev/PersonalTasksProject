namespace PersonalTasksProject.Repositories.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    void AddAsync(T entity);
    void UpdateAsync(T entity);
    void DeleteAsync(T entity);
}