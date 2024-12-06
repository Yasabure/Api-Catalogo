using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public interface IRepository<T>
    {
        // Tomar Cuidado para não violar o Principio ISP
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
