using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Interfaces;
using TallerMecanico.Models;
using TallerMecanico.ViewModels;

namespace TallerMecanico.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosApiController : ControllerBase
    {
        private readonly IRepository<Producto> _repository;

        public ProductosApiController(IRepository<Producto> repository)
        {
            _repository = repository;
        }

        [HttpGet]
public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
{
    var productos = await _repository.GetAllAsync();

    // Le pasamos 10 como Stock por defecto (p.Stock -> 10)
    var dtos = productos.Select(p => new ProductoDto(p.Id, p.Nombre, p.Precio, 10));

    return Ok(dtos);
}
    }
}