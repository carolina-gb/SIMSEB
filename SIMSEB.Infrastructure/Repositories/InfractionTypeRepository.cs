using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Infrastructure.Persistence;

namespace SIMSEB.Infrastructure.Repositories
{
    public class InfractionTypeRepository : IInfractionTypeRepository
    {
        private readonly AppDbContext _context;

        public InfractionTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<InfractionsType?> GetByIdAsync(int id)
        {
            return await _context.InfractionsTypes.FirstOrDefaultAsync(t => t.InfractionTypeId == id);
        }
    }
}
