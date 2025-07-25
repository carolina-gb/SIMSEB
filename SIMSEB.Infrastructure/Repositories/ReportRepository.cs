﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Domain.Entities;
using SIMSEB.Infrastructure.Persistence;

namespace SIMSEB.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid userId, string description, int typeId, Guid evidenceFileId)
        {
            return await _context.Reports.AnyAsync(r =>
                r.UserId == userId &&
                r.Description == description &&
                r.TypeId == typeId &&
                r.EvidenceFileId == evidenceFileId
            );
        }

        public async Task<UserBasicInfo?> GetUserBasicAsync(Guid userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserBasicInfo
                {
                    Name = u.Name,
                    LastName = u.LastName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetReportTypeNameAsync(int typeId)
        {
            return await _context.ReportsTypes
                .Where(rt => rt.ReportTypeId == typeId)
                .Select(rt => rt.ShowName)
                .FirstOrDefaultAsync();
        }

        public async Task<string?> GetStageNameAsync(int stageId)
        {
            return await _context.ReportsStages
                .Where(s => s.ReportStageId == stageId)
                .Select(s => s.ShowName)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Report>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reports
                .Include(r => r.Type)
                .Include(r => r.Stage)
                .Include(r => r.EvidenceFile)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<Report>> GetByCaseNumberAsync(string caseNumber)
        {
            return await _context.Reports
                .Include(r => r.Type)
                .Include(r => r.Stage)
                .Include(r => r.EvidenceFile)
                .Where(r => r.CaseNumber == caseNumber)
                .ToListAsync();
        }
        public async Task<int> CountAllAsync()
        {
            return await _context.Reports.CountAsync();
        }

        public async Task<int> CountByUserIdAsync(Guid userId)
        {
            return await _context.Reports.Where(r => r.UserId == userId).CountAsync();
        }

        public async Task<List<Report>> GetAllPaginatedAsync(int skip, int take)
        {
            return await _context.Reports
                .Include(r => r.Type)
                .Include(r => r.Stage)
                .Include(r => r.EvidenceFile)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<List<Report>> GetByUserIdPaginatedAsync(Guid userId, int skip, int take)
        {
            return await _context.Reports
                .Include(r => r.Type)
                .Include(r => r.Stage)
                .Include(r => r.EvidenceFile)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid reportId)
        {
            return await _context.Reports
                .Include(r => r.Type)
                .Include(r => r.Stage)
                .Include(r => r.EvidenceFile)
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task UpdateAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

    }
}

