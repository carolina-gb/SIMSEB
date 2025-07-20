using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SIMSEB.API.Middleware;
using SIMSEB.Application.Interfaces.Auth;
using SIMSEB.Application.Interfaces.Reports;
using SIMSEB.Application.Interfaces.UserManagement;
using SIMSEB.Application.Interfaces.Users;
using SIMSEB.Application.Services.Auth;
using SIMSEB.Application.Services.UserManagement;
using SIMSEB.Application.Services.Users;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Infrastructure.Persistence;
using SIMSEB.Infrastructure.Repositories;
using SIMSEB.Infrastructure.Seeders;
using SIMSEB.Application.Services.Reports;
using SIMSEB.Application.Interfaces.Infractions;
using SIMSEB.Application.Services.Infractions;
using SIMSEB.Infrastructure.Services;
using SIMSEB.Infrastructure.Hubs;
using SIMSEB.Application.Interfaces.Emergency;
using SIMSEB.Application.Services.Emergency;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// 1. Configuración (JSON + variables de entorno primero que todo)
builder.Configuration
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddEnvironmentVariables();

// -----------------------------------------------------------------------------
// 2. CORS
const string FrontPolicy = "FrontPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontPolicy, policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",              // dev local
                "https://simseb-web.onrender.com",   // frontend en Render
                "https://tu-dominio.com"             // ← agrega más si los necesitas
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// -----------------------------------------------------------------------------
// 3. JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;     // Render sirve HTTPS ✔️
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),
        ClockSkew = TimeSpan.Zero
    };
});

// -----------------------------------------------------------------------------
// 4. Acceso a HttpContext
builder.Services.AddHttpContextAccessor();

// -----------------------------------------------------------------------------
// 5. Servicios del sistema
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IReportTrackingRepository, ReportTrackingRepository>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IInfractionRepository, InfractionRepository>();
builder.Services.AddScoped<IInfractionTypeRepository, InfractionTypeRepository>();
builder.Services.AddScoped<IInfractionService, InfractionService>();
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmergencyRepository, EmergencyRepository>();
builder.Services.AddScoped<IEmergencyService, EmergencyService>();

// -----------------------------------------------------------------------------
// 6. Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------------------------------
// 7. DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// -----------------------------------------------------------------------------
// 8. Build
var app = builder.Build();

// -----------------------------------------------------------------------------
// 9. Dev‑only goodies
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// -----------------------------------------------------------------------------
// 10. Middleware pipeline (orden importa)
app.UseHttpsRedirection();

app.UseCors(FrontPolicy);

// ⚠️ Asegúrate de que tu JwtMiddleware permita las
//     solicitudes OPTIONS para que el pre-flight CORS pase.
/*
public async Task Invoke(HttpContext context)
{
    if (context.Request.Method == HttpMethods.Options)
    {
        await _next(context);
        return;
    }
    ...
}
*/
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hub/notifications");

// -----------------------------------------------------------------------------
// 11. Seeder
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var cfg = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await DbSeeder.SeedAsync(ctx, cfg);
}

app.Run();
