using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TallerMecanico.Data;
using TallerMecanico.Interfaces;
using TallerMecanico.Repositories;
using TallerMecanico.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Conexión a la Base de Datos SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configuración de Identity (Manejo de usuarios, contraseñas y roles)
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false; // Permite ingresar sin confirmar correo
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// 3. Data Seeding: Crear roles y asignar el rol Admin al usuario administrador
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // A. Crear roles si no existen
    string[] roleNames = { "Admin", "Usuario" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // B. Asignar rol Admin al usuario predeterminado (si ya está registrado)
    var adminUser = await userManager.FindByEmailAsync("admin@taller.com");
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// 4. Middlewares y Enrutamiento
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); // 1º Verificar identidad
app.UseAuthorization();  // 2º Verificar permisos y roles

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Redirección directa al Login desde la raíz
app.MapGet("/", context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    return Task.CompletedTask;
});

app.Run();