using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;
namespace SIMSEB.Application.Interfaces.Reports
{
    public interface IFileRepository
    {
        Task CreateAsync(SIMSEB.Domain.Entities.File file);
    }
}
