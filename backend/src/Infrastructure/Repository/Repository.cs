using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Interfaces.Services;

namespace Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PupyDbContext _context;
        private DbSet<T> _entities;

        public Repository(PupyDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<bool> InsertAsync(T entity)
        {
            _entities.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<T?> GetByIdAsync(Guid id) => await _entities.FindAsync(id);

        public async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate) => await _entities.Where(predicate).ToListAsync();

        public async Task<T> UpdateAsync(T entity)
        {
            _entities.Update(entity);
            return await _context.SaveChangesAsync() > 0 ? entity : throw new Exception("Update failed");
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _entities.Remove(entity);
            return await _context.SaveChangesAsync() > 0 ? true : throw new Exception("Delete failed");
        }

    }
}