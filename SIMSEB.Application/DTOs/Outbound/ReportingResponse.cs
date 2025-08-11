using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Outbound
{
    public class ReportingResponseDto
    {
       public ReportSummaryReportDto ReportingReports {  get; set; }  
        public ReportSummaryEmergencyDto ReportingEmergencies { get; set; }
        public ReportSummaryInfraccionDto ReportingInfraccion { get; set; }
    }

    public class ReportSummaryReportDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Clave = tipo de reporte (e.g. "Suciedad", "Seguridad", "Ruido")
        public Dictionary<string, ReporteCategoriaDto> Reportes { get; set; }
    }

    public class ReporteCategoriaDto
    {
        public int ReportesIngresados { get; set; }
        public int ReportesEnCurso { get; set; }
        public int ReportesRechazados { get; set; }
        public int ReportesAprobados { get; set; }

        public int TotalReportes
        {
            get
            {
                return ReportesIngresados + ReportesEnCurso + ReportesRechazados + ReportesAprobados;
            }
        }
    }

    public class ReportSummaryEmergencyDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Clave = tipo de emergencia, Valor = datos de la emergencia
        public Dictionary<string, EmergencyCategoryDto> Emergencias { get; set; }
    }

    public class EmergencyCategoryDto
    {
        public int Total { get; set; }
    }

    public class ReportSummaryInfraccionDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        // Clave: tipo de infracción ("Alta", "Moderada", "Baja")
        public Dictionary<string, InfraccionCategoriaDto> Infracciones { get; set; }
    }

    public class InfraccionCategoriaDto
    {
        public int TotalInfracciones { get; set; }
        public decimal? MontoTotal { get; set; }
    }
}
