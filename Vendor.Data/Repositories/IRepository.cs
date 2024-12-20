using System.Linq.Expressions;

namespace Vendor.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Entity();

        IQueryable<T> Entity(Expression<Func<T, bool>> query);

        Task<List<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
