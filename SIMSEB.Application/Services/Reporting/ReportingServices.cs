using Microsoft.AspNetCore.Http;
using SIMSEB.Application.DTOs.Inbound;
using SIMSEB.Application.DTOs.Outbound;
using SIMSEB.Application.DTOs.Outbound.Response;
using SIMSEB.Application.Interfaces.Reporting;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IEmergencyRepository _emergencyRepository;
        private readonly IInfractionRepository _infractionRepository;

        public ReportingService(
            IReportRepository reportRepository, IEmergencyRepository emergencyRepository, IInfractionRepository infractionRepository)
        {
            _reportRepository = reportRepository;
            _emergencyRepository = emergencyRepository;
            _infractionRepository = infractionRepository;
        }
        public async Task<GeneralResponse<ReportingResponseDto>> GetReportAsync(ReportingRequest request)
        {
            var response = new ReportingResponseDto();

            switch (request.typeReport.ToLower())
            {
                case "reporte":
                    response.ReportingReports = await GetReportSummaryReportsAsync(request.initialDate, request.endDate);
                    break;

                case "emergencia":
                    response.ReportingEmergencies = await GetReportSummaryEmergenciesAsync(request.initialDate, request.endDate);
                    break;

                case "infraccion":
                    response.ReportingInfraccion = await GetReportSummaryInfraccionesAsync(request.initialDate, request.endDate);
                    break;

                default:
                    return new GeneralResponse<ReportingResponseDto>
                    {
                        Code = 404,
                        Message = "No se encontro ese tipo de reporte.",
                        Data = null
                    };
            }
            return new GeneralResponse<ReportingResponseDto>
            {
                Code = 201,
                Message = "Reporte creado exitosamente.",
                Data = response
            };
        }

        private async Task<ReportSummaryReportDto> GetReportSummaryReportsAsync(DateTime inicio, DateTime fin)
        {
            // Obtener todos los reportes en el rango de fechas
            var reports = await _reportRepository.GetByFechaAsync(inicio, fin);
            // o crea un método GetByFechaAsync
            reports = reports.Where(r => r.CreatedAt >= inicio && r.CreatedAt <= fin).ToList();

            // Agrupar por tipo (e.g., "Suciedad", "Seguridad", "Ruido")
            var grouped = reports
                .GroupBy(r => r.Type.ShowName)
                .ToDictionary(
                    g => g.Key,
                    g =>
                    {
                        return new ReporteCategoriaDto
                        {
                            ReportesIngresados = g.Count(r => r.Stage.Name.Equals("opened", StringComparison.OrdinalIgnoreCase)),
                            ReportesEnCurso = g.Count(r => r.Stage.Name.Equals("in_course", StringComparison.OrdinalIgnoreCase)),
                            ReportesRechazados = g.Count(r => r.Stage.Name.Equals("rejected", StringComparison.OrdinalIgnoreCase)),
                            ReportesAprobados = g.Count(r => r.Stage.Name.Equals("approved", StringComparison.OrdinalIgnoreCase))
                        };
                    });

            var dto = new ReportSummaryReportDto
            {
                FechaInicio = inicio,
                FechaFin = fin,
                Reportes = grouped
            };

            return dto;
        }


        private async Task<ReportSummaryEmergencyDto> GetReportSummaryEmergenciesAsync(DateTime inicio, DateTime fin)
        {
            // Obtener todas las emergencias en el rango de fechas
            var emergencias = await _emergencyRepository.GetByFechaAsync(inicio, fin);

            // Agrupar por tipo (e.g., "Incendio", "Pelea", "Robo")
            var agrupadas = emergencias
                .GroupBy(e => e.Type.ShowName)
                .ToDictionary(
                    g => g.Key,
                    g => new EmergencyCategoryDto
                    {
                        Total = g.Count()
                    });

            // Construir el DTO de respuesta
            return new ReportSummaryEmergencyDto
            {
                FechaInicio = inicio,
                FechaFin = fin,
                Emergencias = agrupadas
            };
        }


        private async Task<ReportSummaryInfraccionDto> GetReportSummaryInfraccionesAsync(DateTime inicio, DateTime fin)
        {
            var infracciones = await _infractionRepository.GetByFechaAsync(inicio, fin);

            // Agrupar por severidad
            var agrupadas = infracciones
                .GroupBy(i => i.Type.ShowName) // Asegúrate que i.Type.ShowName representa la severidad
                .ToDictionary(
                    g => g.Key,
                    g => new InfraccionCategoriaDto
                    {
                        TotalInfracciones = g.Count(),
                        MontoTotal = g.Sum(i => i.Amount)
                    });

            return new ReportSummaryInfraccionDto
            {
                FechaInicio = inicio,
                FechaFin = fin,
                Infracciones = agrupadas
            };
        }

    }

}
