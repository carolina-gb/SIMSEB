using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Domain.Entities;
using SIMSEB.Infrastructure.Persistence;

namespace SIMSEB.Infrastructure.Repositories
{
    public class ReportTrackingRepository : IReportTrackingRepository
    {
        private readonly AppDbContext _context;

        public ReportTrackingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ReportsTracking tracking)
        {
            await _context.ReportsTrackings.AddAsync(tracking);
            await _context.SaveChangesAsync();
        }
    }
}
