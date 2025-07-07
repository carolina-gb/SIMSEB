using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class ReportListByUserIdResponseDto
    {
        public List<ReportDto> Data { get; set; } = new();
    }

    public class ReportDto
    {
        public Guid ReportId { get; set; }
        public string CaseNumber { get; set; }
        public string Description { get; set; }
        public string RejectReason { get; set; }
        public Guid? RejectBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public FileDto EvidenceFile { get; set; }
        public ReportTypeDto Type { get; set; }
        public ReportStageDto Stage { get; set; }
    }

    public class FileDto
    {
        public Guid FileId { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class ReportTypeDto
    {
        public int ReportTypeId { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class ReportStageDto
    {
        public int ReportStageId { get; set; }
        public string Name { get; set; }
        public string ShowName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
