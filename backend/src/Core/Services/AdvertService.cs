using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly IRepository<Advert> _advertRepository;
        public AdvertService(IRepository<Advert> advertRepository)
        {
            _advertRepository = advertRepository;
        }

        public async Task<bool> InsertAsync(Advert advert) => await _advertRepository.InsertAsync(advert);
        public async Task<Advert?> GetByIdAsync(Guid id) => await _advertRepository.GetByIdAsync(id);
        public async Task<IEnumerable<Advert>> GetAllAsync(Expression<Func<Advert, bool>> predicate) => await _advertRepository.GetListAsync(predicate);
        public async Task<bool> DeleteAsync(Advert advert) => await _advertRepository.DeleteAsync(advert);
        public async Task<Advert> UpdateAsync(Advert advert) => await _advertRepository.UpdateAsync(advert);

    }
}


