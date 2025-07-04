using Microsoft.EntityFrameworkCore;
using SIMSEB.API.Middleware;
using SIMSEB.Application.Interfaces.Auth;
using SIMSEB.Application.Interfaces.UserManagement;
using SIMSEB.Application.Interfaces.Users;
using SIMSEB.Application.Services.Auth;
using SIMSEB.Application.Services.UserManagement;
using SIMSEB.Application.Services.Users;
using SIMSEB.Domain.Interfaces;
using SIMSEB.Infrastructure.Persistence;
using SIMSEB.Infrastructure.Repositories;
using SIMSEB.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder
                .WithOrigins("http://localhost:4200") // O usa .AllowAnyOrigin() para testing
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Registrar servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();
// Registrar controladores y Swagger
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins); // <--- ¡Ponlo aquí, tempranito!

app.UseMiddleware<JwtMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await DbSeeder.SeedAsync(context, configuration);
}
app.Run();
