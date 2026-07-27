using Microsoft.EntityFrameworkCore;
using TallerMecanico.Data;
using TallerMecanico.Interfaces;
using TallerMecanico.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar servicios MVC (Controladores + Vistas Razor)
builder.Services.AddControllersWithViews();

// 2. Base de datos en memoria
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TallerDb"));

// 3. Registrar el Repositorio Generico (Inyeccion de Dependencias)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 4. Configurar la ruta por defecto (para que abra Productos al iniciar)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Index}/{id?}");

// 5. ¡MUY IMPORTANTE! Esta linea mantiene el servidor corriendo
app.Run();