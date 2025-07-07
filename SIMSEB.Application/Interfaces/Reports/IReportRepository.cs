using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Application.Interfaces.Reports
{
    public interface IReportRepository
    {
        Task CreateAsync(Report report);
        Task<bool> ExistsAsync(Guid userId, string description, int typeId, Guid evidenceFileId);
        Task<string?> GetReportTypeNameAsync(int typeId);
        Task<string?> GetStageNameAsync(int stageId);
        Task<UserBasicInfo?> GetUserBasicAsync(Guid userId);
    }

    public class UserBasicInfo
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
