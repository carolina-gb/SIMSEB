using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Infrastructure.Persistence;

namespace SIMSEB.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(SIMSEB.Domain.Entities.File file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }
    }
}
