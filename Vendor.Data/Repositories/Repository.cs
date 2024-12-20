using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Vendor.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly VendorContext _db;

        public Repository(VendorContext db)
        {
            _db = db;
        }

        public IQueryable<T> Entity()
        {
            return _db.Set<T>().AsNoTracking();
        }

        public IQueryable<T> Entity(Expression<Func<T, bool>> query)
        {
            return _db.Set<T>().Where(query).AsNoTracking();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task CreateAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
