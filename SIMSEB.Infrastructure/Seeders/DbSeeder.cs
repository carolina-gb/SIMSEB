using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SIMSEB.Domain.Entities;
using SIMSEB.Domain.Utils;
using SIMSEB.Infrastructure.Persistence;


namespace SIMSEB.Infrastructure.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IConfiguration configuration)
        {
            if (!await context.Users.AnyAsync())
            {
                var adminConfig = configuration.GetSection("AdminSeed");
                var username = adminConfig["Username"];
                var email = adminConfig["Email"];
                var password = adminConfig["Password"];
                
                //Cifrado de contraseña
                var hashedPassword = HashHelper.Hash(password!);

                var admin = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = username!,
                    Name = "Administrador",
                    LastName = "Sistema",
                    Identification = "0000000000",
                    Email = email!,
                    Password = hashedPassword,
                    PasswordHint = hashedPassword,
                    TypeId = 1,
                    Status = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    DeletedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
