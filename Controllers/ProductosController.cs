using Microsoft.AspNetCore.Mvc;
using TallerMecanico.Interfaces;
using TallerMecanico.Models;

namespace TallerMecanico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IRepository<Producto> _repository;

    public ProductosController(IRepository<Producto> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var productos = await _repository.GetAllAsync();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _repository.GetByIdAsync(id);
        if (producto == null) return NotFound();
        return Ok(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Producto producto)
    {
        await _repository.AddAsync(producto);
        await _repository.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Producto producto)
    {
        if (id != producto.Id) return BadRequest();

        _repository.Update(producto);
        await _repository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _repository.GetByIdAsync(id);
        if (producto == null) return NotFound();

        _repository.Remove(producto);
        await _repository.SaveChangesAsync();
        return NoContent();
    }
}