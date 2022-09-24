using Core.Entities;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<bool> InsertAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        Task<bool> DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
    }
}