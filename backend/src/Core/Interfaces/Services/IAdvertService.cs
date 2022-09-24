using Core.Entities;
using System.Linq.Expressions;

namespace Core.Interfaces.Services
{
    public interface IAdvertService
    {
        Task<bool> InsertAsync(Advert advert);
        Task<bool> DeleteAsync(Advert advert);
        Task<IEnumerable<Advert>> GetAllAsync(Expression<Func<Advert, bool>> predicate);
        Task<Advert?> GetByIdAsync(Guid id);
        Task<Advert> UpdateAsync(Advert advert);
    }
}