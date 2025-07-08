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
        Task<List<Report>> GetByUserIdAsync(Guid userId);
        Task<List<Report>> GetByCaseNumberAsync(string caseNumber);
        Task<int> CountAllAsync();
        Task<int> CountByUserIdAsync(Guid userId);
        Task<List<Report>> GetAllPaginatedAsync(int skip, int take);
        Task<List<Report>> GetByUserIdPaginatedAsync(Guid userId, int skip, int take);
        Task<Report?> GetByIdAsync(Guid reportId);
        Task UpdateAsync(Report report);


    }

    public class UserBasicInfo
    {
        public string Name { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
