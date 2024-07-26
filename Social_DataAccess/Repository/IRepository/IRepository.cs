using System.Linq.Expressions;

namespace Social_DataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task <IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null);
    Task<T> Get(Expression<Func<T, bool>> filter);
    Task<T> Create(T entity);
    Task<bool> Delete(T entity);
}