using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;
using EcommerceBackend.Services;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReseñaController : ControllerBase
    {
        private readonly IReseñaService _reseñaService;

        public ReseñaController(IReseñaService reseñaService)
        {
            _reseñaService = reseñaService;
        }

        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<List<Reseña>>> GetByProductoId(int productoId)
        {
            var reseñas = await _reseñaService.GetByProductoIdAsync(productoId);
            return Ok(reseñas);
        }

        [HttpGet("producto/{productoId}/promedio")]
        public async Task<ActionResult<double>> GetPromedioCalificacion(int productoId)
        {
            try
            {
                var promedio = await _reseñaService.GetPromedioCalificacionAsync(productoId);
                return Ok(promedio);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al obtener el promedio de calificación: {ex.Message}");
                return StatusCode(500, $"Error interno al calcular el promedio: {ex.Message}");
            }
        }



        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Reseña>> AddReseña([FromBody] Reseña reseña)
        {
            if (reseña == null || reseña.UsuarioId <= 0)
            {
                return BadRequest("Datos de reseña inválidos.");
            }

            await _reseñaService.AddAsync(reseña);
            return Ok(reseña);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReseña(int id)
        {
            await _reseñaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
