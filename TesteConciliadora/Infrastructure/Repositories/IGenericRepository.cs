using System.Linq.Expressions;

namespace TesteConciliadora.Infrastructure.Repositories;


public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task Add(T entity);
    Task<List<T>> Where(Expression<Func<T, bool>> predicate);
}