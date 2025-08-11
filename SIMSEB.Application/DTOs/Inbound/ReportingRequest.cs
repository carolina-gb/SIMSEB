using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class ReportingRequest
    {
        public DateTime initialDate {  get; set; }
        public DateTime endDate { get; set; }
        public String typeReport {  get; set; }
    }
}
