using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Domain.Interfaces
{
    public interface IInfractionRepository
    {
        Task CreateAsync(Infraction infraction);
        Task<string> GenerateNextInfractionNumberAsync();
        Task<Infraction?> GetByIdAsync(Guid id);
        Task UpdateAsync(Infraction infraction);
        Task<Infraction?> GetDetailedByIdAsync(Guid id);
        Task<Infraction?> GetDetailedByNumberAsync(string infractionNumber);
        Task<List<Infraction>> GetAllDetailedAsync();

    }
}
