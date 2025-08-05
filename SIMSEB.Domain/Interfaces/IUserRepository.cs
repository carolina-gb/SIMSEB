using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailOrUsernameAsync(string email);
        Task<User?> GetByIdentificationAsync(string identification);
        Task UpdateAsync(User user);

        Task<IEnumerable<User>> GetByTypeIdsAsync(IEnumerable<int> typeIds);

        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetDetailedByFilterAsync(string filter);
    }
}
