using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Extensions;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDto>>> GetAll()
        {
            try
            {
                var productos = await _productoService.GetAllAsync();
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos: {ex.Message}" });
            }
        }

        [HttpGet("paginado")]
        public async Task<ActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (productos, total, pages) = await _productoService.GetPaginatedAsync(page, pageSize);
                var dtos = productos.ConvertAll(p => p.ToDto());
                
                return Ok(new {
                    productos = dtos,
                    total,
                    page,
                    pageSize,
                    pages
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos paginados: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            try
            {
                var producto = await _productoService.GetByIdAsync(id);
                return Ok(producto.ToDto());
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener el producto: {ex.Message}" });
            }
        }

        [HttpGet("categoria/{categoriaId}")]
        public async Task<ActionResult<List<ProductoDto>>> GetByCategoria(int categoriaId)
        {
            try
            {
                var productos = await _productoService.GetByCategoriaIdAsync(categoriaId);
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al obtener productos por categoría: {ex.Message}" });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductoDto>>> SearchByName([FromQuery] string query)
        {
            try
            {
                var productos = await _productoService.SearchByNameAsync(query);
                var dtos = productos.ConvertAll(p => p.ToDto());
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al buscar productos: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] Producto producto)
        {
            try
            {
                await _productoService.AddAsync(producto);
                return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al crear el producto: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Producto producto)
        {
            try
            {
                producto.Id = id;
                await _productoService.UpdateAsync(producto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al actualizar el producto: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Error al eliminar el producto: {ex.Message}" });
            }
        }
    }
}
