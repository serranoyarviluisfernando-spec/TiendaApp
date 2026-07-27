using Microsoft.EntityFrameworkCore;
using TallerMecanico.Models;

namespace TallerMecanico.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Producto> Productos { get; set; }
}