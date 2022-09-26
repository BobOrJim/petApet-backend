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
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> InsertAsync(User user) => await _userRepository.InsertAsync(user);
        public async Task<User?> GetByIdAsync(Guid id) => await _userRepository.GetByIdAsync(id);
        public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate) => await _userRepository.GetListAsync(predicate);
        public async Task<bool> DeleteAsync(User user) => await _userRepository.DeleteAsync(user);
        public async Task<User> UpdateAsync(User user) => await _userRepository.UpdateAsync(user);



    }
}


