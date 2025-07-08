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
    public class EmergencyRepository : IEmergencyRepository
    {
        private readonly AppDbContext _context;

        public EmergencyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Emergency emergency)
        {
            _context.Emergencies.Add(emergency);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Emergency>> GetAllAsync()
        {
            return await _context.Emergencies
                .Include(e => e.Type)
                .Include(e => e.User)
                .ToListAsync();
        }

        public async Task<List<Emergency>> GetAllDetailedAsync(int typeId, Guid userId, int skip, int take)
        {
            var query = _context.Emergencies
                .Include(e => e.Type)
                .Include(e => e.User)
                .AsQueryable();

            // Si no es superadmin o admin (typeId != 1 ni 2), filtrar por su userId
            if (typeId != 1 && typeId != 2)
            {
                query = query.Where(e => e.UserId == userId);
            }

            return await query
                .OrderByDescending(e => e.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> CountAsync(int typeId, Guid userId)
        {
            var query = _context.Emergencies.AsQueryable();

            if (typeId != 1 && typeId != 2)
            {
                query = query.Where(e => e.UserId == userId);
            }

            return await query.CountAsync();
        }
    }
}