using Core.Entities;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> InsertAsync(User user);
        Task<bool> DeleteAsync(User user);
        Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate);
        Task<User?> GetByIdAsync(Guid id);
        Task<User> UpdateAsync(User user);
    }
}