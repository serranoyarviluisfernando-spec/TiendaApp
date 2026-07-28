using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Interfaces;
using TallerMecanico.Models;
using TallerMecanico.ViewModels;

namespace TallerMecanico.Controllers
{
    [Authorize] //  Requiere iniciar sesión para acceder a cualquier vista de Productos
    public class ProductosController : Controller
    {
        private readonly IRepository<Producto> _repository;

        public ProductosController(IRepository<Producto> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var productos = await _repository.GetAllAsync();
            return View(productos);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] //  Solo los usuarios con rol 'Admin' pueden acceder al formulario
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recibe el ViewModel en lugar de la Entidad directa
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] //  Solo administradores pueden guardar productos
        public async Task<IActionResult> Create(ProductoCreateViewModel vm)
        {
            // 1. Validar el estado del Modelo (Servidor)
            if (!ModelState.IsValid)
            {
                return View(vm); // Retorna a la vista mostrando los errores
            }

            // 2. Mapear de ViewModel a la Entidad real
            var nuevoProducto = new Producto
            {
                Nombre = vm.Nombre,
                Precio = vm.Precio
            };

            // 3. Guardar en Base de Datos
            await _repository.AddAsync(nuevoProducto);
            await _repository.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}