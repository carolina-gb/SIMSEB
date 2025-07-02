using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMSEB.Domain.Entities;

namespace SIMSEB.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailOrUsernameAsync(string email);
    }
}
