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
        public async Task<IEnumerable<User>> GetDetailedByFilterAsync(string filter)
        {
            // Asegúrate de que filter no sea null ni esté vacío (opcional)
            if (string.IsNullOrWhiteSpace(filter))
                return new List<User>();

            // Lo pasamos a minúsculas para hacer búsqueda case-insensitive (opcional)
            filter = filter.Trim().ToLower();

            return await _context.Users
                .Include(u => u.Type)
                .Include(u => u.StatusNavigation)
                .Where(u =>
                    u.Username.ToLower().Contains(filter) ||
                    u.Name.ToLower().Contains(filter) ||
                    u.LastName.ToLower().Contains(filter)
                )
                .ToListAsync();
        }
    }
}