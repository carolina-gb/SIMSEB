using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Domain.Interfaces
{
    public interface IEmergencyRepository
    {
        Task AddAsync(Emergency emergency);
        Task<List<Emergency>> GetAllDetailedAsync(int typeId, Guid userId, int skip, int take);
        Task<int> CountAsync(int typeId, Guid userId);
    }
}
