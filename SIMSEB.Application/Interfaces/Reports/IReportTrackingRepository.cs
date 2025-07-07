using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Application.Interfaces.Reports
{
    public interface IReportTrackingRepository
    {
        Task CreateAsync(ReportsTracking tracking);
    }
}
