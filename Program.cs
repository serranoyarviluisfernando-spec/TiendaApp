using Microsoft.EntityFrameworkCore;
using TallerMecanico.Data;
using TallerMecanico.Interfaces;
using TallerMecanico.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Base de datos en memoria
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TallerDb"));

// Registro del Patrón Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
// ... resto de tu configuración (Swagger, etc.)