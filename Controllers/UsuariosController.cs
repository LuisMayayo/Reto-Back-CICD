using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using System.Security.Claims;
using System.Linq; // Necesario para el Select

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuario/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EsAdmin = usuario.EsAdmin,
                Telefono = usuario.Telefono,  // Se incluye teléfono
                Direccion = usuario.Direccion // Se incluye dirección
            };

            return Ok(usuarioDto);
        }

        // GET: api/usuario
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var usuarios = await _usuarioService.GetAllAsync();
            var dtos = usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Email = u.Email,
                FechaRegistro = u.FechaRegistro,
                EsAdmin = u.EsAdmin,
                Telefono = u.Telefono,   // Se incluye teléfono
                Direccion = u.Direccion  // Se incluye dirección
            }).ToList();

            return Ok(dtos);
        }

        // PUT: api/usuario/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            // Verificar autenticación y autorización (como ya se hace)
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userClaimId, out int claimId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }
            var userRole = User.FindFirst("role")?.Value
                ?? User.FindFirst(ClaimTypes.Role)?.Value ?? "No definido";

            if (claimId != id && userRole != "Admin")
                return StatusCode(StatusCodes.Status403Forbidden, "No tienes permiso para modificar este usuario.");

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            // Actualización de campos
            usuario.Nombre = !string.IsNullOrEmpty(request.Nombre) ? request.Nombre : usuario.Nombre;
            usuario.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : usuario.Email;
            usuario.Telefono = !string.IsNullOrEmpty(request.Telefono) ? request.Telefono : usuario.Telefono;
            usuario.Direccion = !string.IsNullOrEmpty(request.Direccion) ? request.Direccion : usuario.Direccion;

            // Si se envía el campo EsAdmin, actualizarlo
            if (request.EsAdmin.HasValue)
            {
                usuario.EsAdmin = request.EsAdmin.Value;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                using (var hmac = new HMACSHA512())
                {
                    usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);
                    usuario.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
                }
            }

            await _usuarioService.UpdateAsync(usuario);
            return NoContent();
        }

        // DELETE: api/usuario/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userClaimId, out int claimId))
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "No se encontró un usuario autenticado.");
            }
            var userRole = User.FindFirst("role")?.Value
                ?? User.FindFirst(ClaimTypes.Role)?.Value ?? "No definido";

            if (claimId != id && userRole != "Admin")
                return StatusCode(StatusCodes.Status403Forbidden, "No tienes permiso para eliminar este usuario.");

            var usuario = await _usuarioService.GetByIdAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult GetAdminData()
        {
            return Ok("Este es un endpoint solo para administradores.");
        }
    }

    public class UpdateUserRequest
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        // Nueva propiedad para actualizar el rol (true = Admin, false = Usuario)
        public bool? EsAdmin { get; set; }
    }

}
