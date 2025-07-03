using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMSEB.Application.DTOs.Inbound
{
    public class LoginRequestDto
    {
        public string Input { get; set; }  // Email o username
        public string Password { get; set; }
    }
}
