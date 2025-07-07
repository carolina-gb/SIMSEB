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
    }
}