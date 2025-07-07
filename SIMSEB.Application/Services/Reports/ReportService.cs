using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace SIMSEB.Application.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IReportTrackingRepository _reportTrackingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReportService(
            IReportRepository reportRepository,
            IFileRepository fileRepository,
            IReportTrackingRepository reportTrackingRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reportRepository = reportRepository;
            _fileRepository = fileRepository;
            _reportTrackingRepository = reportTrackingRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GeneralResponse<CreatedReportResponseDto>> CreateReportAsync(CreateReportRequestDto dto)
        {
            // Obtener userId y typeId desde JWT
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            var userTypeClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("typeId")?.Value;

            if (userIdClaim is null || userTypeClaim != "3")
            {
                return new GeneralResponse<CreatedReportResponseDto>
                {
                    Code = 403,
                    Message = "Solo usuarios de tipo 3 pueden crear reportes.",
                    Data = null
                };
            }

            var userId = Guid.Parse(userIdClaim);

            // Validar formato de archivo
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(dto.EvidenceFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return new GeneralResponse<CreatedReportResponseDto>
                {
                    Code = 400,
                    Message = "El archivo debe ser una imagen JPG o PNG.",
                    Data = null
                };
            }

            // Guardar archivo en disco
            var fileId = Guid.NewGuid();
            var filePath = $"uploads/{fileId}{extension}";
            var fullPath = Path.Combine("wwwroot", filePath);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await dto.EvidenceFile.CopyToAsync(stream);
            }

            // Insertar archivo en tabla files
            var file = new SIMSEB.Domain.Entities.File
            {
                FileId = fileId,
                Path = filePath,
                Type = extension,
                UploadedAt = DateTime.UtcNow
            };
            await _fileRepository.CreateAsync(file);

            // Verificar duplicado
            var exists = await _reportRepository.ExistsAsync(userId, dto.Description, dto.TypeId, fileId);
            if (exists)
            {
                return new GeneralResponse<CreatedReportResponseDto>
                {
                    Code = 409,
                    Message = "Ya existe un reporte con la misma información.",
                    Data = null
                };
            }

            // Crear reporte
            var reportId = Guid.NewGuid();
            var caseNumber = await GenerateNextCaseNumberAsync();

            var report = new Report
            {
                ReportId = reportId,
                CaseNumber = caseNumber,
                TypeId = dto.TypeId,
                EvidenceFileId = fileId,
                UserId = userId,
                Description = dto.Description,
                RejectReason = null,
                RejectBy = null,
                StageId = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _reportRepository.CreateAsync(report);

            // Crear tracking
            var tracking = new ReportsTracking
            {
                ReportId = reportId,
                NewStageId = 1,
                CreatedAt = DateTime.UtcNow
            };
            await _reportTrackingRepository.CreateAsync(tracking);

            // Armar respuesta
            var user = await _reportRepository.GetUserBasicAsync(userId);
            var typeName = await _reportRepository.GetReportTypeNameAsync(dto.TypeId);
            var stageName = await _reportRepository.GetStageNameAsync(1);

            var response = new CreatedReportResponseDto
            {
                ReportId = reportId,
                CaseNumber = caseNumber,
                Description = dto.Description,
                CreatedBy = $"{user?.Name} {user?.LastName}",
                TypeName = typeName ?? "Desconocido",
                StageName = stageName ?? "Ingresado",
                EvidenceFileName = dto.EvidenceFile.FileName,
                CreatedAt = report.CreatedAt
            };

            return new GeneralResponse<CreatedReportResponseDto>
            {
                Code = 201,
                Message = "Reporte creado exitosamente.",
                Data = response
            };
        }

        public async Task<GeneralResponse<ReportListByUserIdResponseDto>> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                var reports = await _reportRepository.GetByUserIdAsync(userId);

                var dto = new ReportListByUserIdResponseDto
                {
                    Data = reports.Select(report => new ReportDto
                    {
                        ReportId = report.ReportId,
                        CaseNumber = report.CaseNumber,
                        Description = report.Description,
                        RejectReason = report.RejectReason,
                        RejectBy = report.RejectBy, // Asegúrate que en DTO sea Guid?
                        CreatedAt = report.CreatedAt,
                        UpdatedAt = report.UpdatedAt,

                        EvidenceFile = new FileDto
                        {
                            FileId = report.EvidenceFile.FileId,
                            Path = report.EvidenceFile.Path,
                            Type = report.EvidenceFile.Type,
                            UploadedAt = report.EvidenceFile.UploadedAt
                        },
                        Type = new ReportTypeDto
                        {
                            ReportTypeId = report.Type.ReportTypeId,
                            Name = report.Type.Name,
                            ShowName = report.Type.ShowName,
                            CreatedAt = report.Type.CreatedAt
                        },
                        Stage = new ReportStageDto
                        {
                            ReportStageId = report.Stage.ReportStageId,
                            Name = report.Stage.Name,
                            ShowName = report.Stage.ShowName,
                            CreatedAt = report.Stage.CreatedAt
                        }
                    }).ToList()
                };

                return new GeneralResponse<ReportListByUserIdResponseDto>
                {
                    Code = 200,
                    Message = "Reportes obtenidos correctamente.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ReportListByUserIdResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<ReportListByUserIdResponseDto>> GetByCaseNumberAsync(Guid userId, int typeId, string caseNumber)
        {
            try
            {
                List<Report> reports;

                if (typeId == 3)
                {
                    // Usuario regular: primero obtengo solo los suyos
                    reports = await _reportRepository.GetByUserIdAsync(userId);
                    reports = reports.Where(r => r.CaseNumber.Equals(caseNumber, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    // Admin o SuperAdmin: búsqueda libre
                    reports = await _reportRepository.GetByCaseNumberAsync(caseNumber);
                }

                if (!reports.Any())
                {
                    return new GeneralResponse<ReportListByUserIdResponseDto>
                    {
                        Code = 404,
                        Message = "No se encontraron reportes con ese número de caso.",
                        Data = null
                    };
                }

                var dto = new ReportListByUserIdResponseDto
                {
                    Data = reports.Select(report => new ReportDto
                    {
                        ReportId = report.ReportId,
                        CaseNumber = report.CaseNumber,
                        Description = report.Description,
                        RejectReason = report.RejectReason,
                        RejectBy = report.RejectBy,
                        CreatedAt = report.CreatedAt,
                        UpdatedAt = report.UpdatedAt,

                        EvidenceFile = new FileDto
                        {
                            FileId = report.EvidenceFile.FileId,
                            Path = report.EvidenceFile.Path,
                            Type = report.EvidenceFile.Type,
                            UploadedAt = report.EvidenceFile.UploadedAt
                        },
                        Type = new ReportTypeDto
                        {
                            ReportTypeId = report.Type.ReportTypeId,
                            Name = report.Type.Name,
                            ShowName = report.Type.ShowName,
                            CreatedAt = report.Type.CreatedAt
                        },
                        Stage = new ReportStageDto
                        {
                            ReportStageId = report.Stage.ReportStageId,
                            Name = report.Stage.Name,
                            ShowName = report.Stage.ShowName,
                            CreatedAt = report.Stage.CreatedAt
                        }
                    }).ToList()
                };

                return new GeneralResponse<ReportListByUserIdResponseDto>
                {
                    Code = 200,
                    Message = "Reportes filtrados por número de caso.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ReportListByUserIdResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<ReportPaginatedResponseDto>> GetAllReportsPaginatedAsync(Guid userId, int typeId, int skip)
        {
            try
            {
                const int take = 5;
                List<Report> reports;
                int count;

                if (typeId == 3)
                {
                    reports = await _reportRepository.GetByUserIdPaginatedAsync(userId, skip, take);
                    count = await _reportRepository.CountByUserIdAsync(userId);
                }
                else
                {
                    reports = await _reportRepository.GetAllPaginatedAsync(skip, take);
                    count = await _reportRepository.CountAllAsync();
                }

                var dto = new ReportPaginatedResponseDto
                {
                    Count = count,
                    Data = reports.Select(report => new ReportDto
                    {
                        ReportId = report.ReportId,
                        CaseNumber = report.CaseNumber,
                        Description = report.Description,
                        RejectReason = report.RejectReason,
                        RejectBy = report.RejectBy,
                        CreatedAt = report.CreatedAt,
                        UpdatedAt = report.UpdatedAt,
                        EvidenceFile = new FileDto
                        {
                            FileId = report.EvidenceFile.FileId,
                            Path = report.EvidenceFile.Path,
                            Type = report.EvidenceFile.Type,
                            UploadedAt = report.EvidenceFile.UploadedAt
                        },
                        Type = new ReportTypeDto
                        {
                            ReportTypeId = report.Type.ReportTypeId,
                            Name = report.Type.Name,
                            ShowName = report.Type.ShowName,
                            CreatedAt = report.Type.CreatedAt
                        },
                        Stage = new ReportStageDto
                        {
                            ReportStageId = report.Stage.ReportStageId,
                            Name = report.Stage.Name,
                            ShowName = report.Stage.ShowName,
                            CreatedAt = report.Stage.CreatedAt
                        }
                    }).ToList()
                };

                return new GeneralResponse<ReportPaginatedResponseDto>
                {
                    Code = 200,
                    Message = "Reportes paginados correctamente.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ReportPaginatedResponseDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<ReportDto>> GetByReportIdAsync(Guid reportId, Guid userId, int typeId)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(reportId);

                if (report == null)
                {
                    return new GeneralResponse<ReportDto>
                    {
                        Code = 404,
                        Message = "Reporte no encontrado.",
                        Data = null
                    };
                }

                // Validación solo si es tipo 3
                if (typeId == 3 && report.UserId != userId)
                {
                    return new GeneralResponse<ReportDto>
                    {
                        Code = 403,
                        Message = "No tiene permisos para ver este reporte.",
                        Data = null
                    };
                }

                var dto = new ReportDto
                {
                    ReportId = report.ReportId,
                    CaseNumber = report.CaseNumber,
                    Description = report.Description,
                    RejectReason = report.RejectReason,
                    RejectBy = report.RejectBy,
                    CreatedAt = report.CreatedAt,
                    UpdatedAt = report.UpdatedAt,
                    EvidenceFile = new FileDto
                    {
                        FileId = report.EvidenceFile.FileId,
                        Path = report.EvidenceFile.Path,
                        Type = report.EvidenceFile.Type,
                        UploadedAt = report.EvidenceFile.UploadedAt
                    },
                    Type = new ReportTypeDto
                    {
                        ReportTypeId = report.Type.ReportTypeId,
                        Name = report.Type.Name,
                        ShowName = report.Type.ShowName,
                        CreatedAt = report.Type.CreatedAt
                    },
                    Stage = new ReportStageDto
                    {
                        ReportStageId = report.Stage.ReportStageId,
                        Name = report.Stage.Name,
                        ShowName = report.Stage.ShowName,
                        CreatedAt = report.Stage.CreatedAt
                    }
                };

                return new GeneralResponse<ReportDto>
                {
                    Code = 200,
                    Message = "Reporte obtenido correctamente.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<ReportDto>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<string>> UpdateStageAsync(UpdateReportStageRequestDto dto, Guid userId, int typeId)
        {
            try
            {
                if (typeId != 1 && typeId != 2)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 403,
                        Message = "No tiene permisos para actualizar el estado.",
                        Data = null
                    };
                }

                var report = await _reportRepository.GetByIdAsync(dto.ReportId);
                if (report == null)
                {
                    return new GeneralResponse<string>
                    {
                        Code = 404,
                        Message = "Reporte no encontrado.",
                        Data = null
                    };
                }

                // Validación si es rechazo
                var REJECTED_STAGE_ID = 3; // reemplaza por el ID real de "Rechazado"
                if (dto.StageId == REJECTED_STAGE_ID && string.IsNullOrWhiteSpace(dto.RejectReason))
                {
                    return new GeneralResponse<string>
                    {
                        Code = 400,
                        Message = "Debe ingresar una razón de rechazo.",
                        Data = null
                    };
                }

                // Actualizamos los campos
                report.StageId = dto.StageId;
                report.UpdatedAt = DateTime.UtcNow;

                if (dto.StageId == REJECTED_STAGE_ID)
                {
                    report.RejectReason = dto.RejectReason;
                    report.RejectBy = userId;
                }

                await _reportRepository.UpdateAsync(report);

                return new GeneralResponse<string>
                {
                    Code = 200,
                    Message = "Estado actualizado correctamente.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<string>
                {
                    Code = 500,
                    Message = $"Error interno: {ex.Message}",
                    Data = null
                };
            }
        }




        // Simulación de generación de case_number (secuencial)
        private async Task<string> GenerateNextCaseNumberAsync()
        {
            // ⚠️ Esta lógica debería ir idealmente en base de datos o repositorio
            var nextNumber = new Random().Next(1, 9999); // TEMPORAL
            return $"REPO-{nextNumber:D4}";
        }
    }
}