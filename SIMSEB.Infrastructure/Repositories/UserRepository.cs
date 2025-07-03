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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailOrUsernameAsync(string input)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == input || u.Username == input);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetByTypeIdsAsync(IEnumerable<int> typeIds)
        {
            return await _context.Users
                .Where(u => typeIds.Contains(u.TypeId))
                .Include(u => u.StatusNavigation)
                .Include(u => u.Type)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Type)
                .Include(u => u.StatusNavigation)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetDetailedByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Type)
                .Include(u => u.StatusNavigation)
                .FirstOrDefaultAsync(u => u.Username == username);
        }


    }
}