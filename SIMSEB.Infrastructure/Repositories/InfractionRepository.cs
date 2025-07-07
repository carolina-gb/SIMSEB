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
    public class InfractionRepository : IInfractionRepository
    {
        private readonly AppDbContext _context;

        public InfractionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Infraction infraction)
        {
            await _context.Infractions.AddAsync(infraction);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GenerateNextInfractionNumberAsync()
        {
            var lastNumber = await _context.Infractions
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.InfractionNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastNumber) && lastNumber.StartsWith("INF-"))
            {
                var parts = lastNumber.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var parsed))
                    nextNumber = parsed + 1;
            }

            return $"INF-{DateTime.UtcNow.Year}-{nextNumber.ToString("D6")}";
        }
        public async Task<Infraction?> GetByIdAsync(Guid id)
        {
            return await _context.Infractions.FirstOrDefaultAsync(i => i.InfractionId == id);
        }

        public async Task UpdateAsync(Infraction infraction)
        {
            _context.Infractions.Update(infraction);
            await _context.SaveChangesAsync();
        }

        public async Task<Infraction?> GetDetailedByIdAsync(Guid id)
        {
            return await _context.Infractions
                .Include(i => i.User)
                    .ThenInclude(u => u.Type)
                .Include(i => i.User)
                    .ThenInclude(u => u.StatusNavigation)
                .Include(i => i.Type)
                .FirstOrDefaultAsync(i => i.InfractionId == id);
        }

        public async Task<Infraction?> GetDetailedByNumberAsync(string infractionNumber)
        {
            return await _context.Infractions
                .Include(i => i.User)
                    .ThenInclude(u => u.Type)
                .Include(i => i.User)
                    .ThenInclude(u => u.StatusNavigation)
                .Include(i => i.Type)
                .FirstOrDefaultAsync(i => i.InfractionNumber == infractionNumber);
        }
        public async Task<List<Infraction>> GetAllDetailedAsync()
        {
            return await _context.Infractions
                .Include(i => i.User)
                    .ThenInclude(u => u.Type)
                .Include(i => i.User)
                    .ThenInclude(u => u.StatusNavigation)
                .Include(i => i.Type)
                .ToListAsync();
        }

    }
}