using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;
using EcommerceBackend.Services;
using System.IdentityModel.Tokens.Jwt;
using Google.Apis.Auth;


namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;

        public AuthController(IUsuarioService usuarioService, JwtService jwtService, IEmailService emailService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UsuarioDto>> Register([FromBody] RegisterRequest request)
        {
            var emailPattern = @"^[^\s@]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(request.Email, emailPattern))
                return BadRequest("El correo electrónico no es válido.");

            var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(request.Password, passwordPattern))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.");

            var existingUser = await _usuarioService.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return Conflict("El usuario ya existe con ese correo.");

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                FechaRegistro = DateTime.UtcNow,
                EsAdmin = request.EsAdmin // Nuevo campo en RegisterRequest
            };

            using (var hmac = new HMACSHA512())
            {
                usuario.PasswordSalt = Convert.ToBase64String(hmac.Key);
                usuario.PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
            }

            await _usuarioService.AddAsync(usuario);

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                FechaRegistro = usuario.FechaRegistro,
                EsAdmin = usuario.EsAdmin
            };

            return CreatedAtAction(nameof(UsuarioController.GetById), "Usuario", new { id = usuario.Id }, usuarioDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioService.GetByEmailAsync(request.Email);
            if (usuario == null)
                return Unauthorized("Usuario o contraseña incorrectos.");

            using (var hmac = new HMACSHA512(Convert.FromBase64String(usuario.PasswordSalt)))
            {
                var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)));
                if (computedHash != usuario.PasswordHash)
                    return Unauthorized("Usuario o contraseña incorrectos.");
            }

            var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.EsAdmin);
            return Ok(new { Token = token, EsAdmin = usuario.EsAdmin });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var usuario = await _usuarioService.GetByEmailAsync(request.Email);
            if (usuario == null)
                return Ok("Si el correo existe, se enviará un enlace para restablecer la contraseña.");

            // Generate a unique reset token
            var resetToken = Guid.NewGuid().ToString();
            usuario.ResetToken = resetToken;
            usuario.ResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

            // Save changes to the database
            await _usuarioService.UpdateAsync(usuario);

            // Send email with reset link
            var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";
            var subject = "Restablecer contraseña";
            var body = $"Hola {usuario.Nombre},\n\nHaz clic en el siguiente enlace para restablecer tu contraseña:\n{resetLink}\n\nSi no solicitaste esto, ignora este mensaje.";
            await _emailService.SendEmailAsync(usuario.Email, subject, body);

            return Ok("Se envió un enlace para restablecer la contraseña (si el correo existe).");
        }



        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Validate token format
            if (string.IsNullOrEmpty(request.Token))
                return BadRequest("Token inválido.");

            // Look up user by reset token
            var usuario = await _usuarioService.GetByResetTokenAsync(request.Token);
            if (usuario == null || usuario.ResetTokenExpiration < DateTime.UtcNow)
                return BadRequest("Token inválido o expirado.");

            // Validate new password format
            var passwordPattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!Regex.IsMatch(request.NewPassword, passwordPattern))
                return BadRequest("La contraseña debe tener al menos 8 caracteres, una mayúscula, un número y un carácter especial.");

            // Hash the new password
            string newPasswordHash;
            string newPasswordSalt;
            using (var hmac = new HMACSHA512())
            {
                newPasswordSalt = Convert.ToBase64String(hmac.Key);
                newPasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(request.NewPassword)));
            }

            // Update user with new password and clear reset token
            usuario.PasswordHash = newPasswordHash;
            usuario.PasswordSalt = newPasswordSalt;
            usuario.ResetToken = null;
            usuario.ResetTokenExpiration = null;

            await _usuarioService.UpdateAsync(usuario);

            return Ok("Contraseña restablecida exitosamente.");
        }



        [HttpPost("google-login")]
        public async Task<ActionResult<object>> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.IdToken,
                    new GoogleJsonWebSignature.ValidationSettings()
                );
                if (payload == null)
                    return Unauthorized("Token de Google inválido.");

                var usuario = await _usuarioService.GetByEmailAsync(payload.Email);
                if (usuario == null)
                {
                    usuario = new Usuario
                    {
                        Nombre = payload.Name ?? "Usuario Google",
                        Email = payload.Email,
                        FechaRegistro = DateTime.UtcNow
                    };
                    await _usuarioService.AddAsync(usuario);
                }

                var token = _jwtService.GenerateToken(usuario.Id, usuario.Email, usuario.EsAdmin);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized("Error al validar token de Google: " + ex.Message);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            return Ok("Cuenta confirmada exitosamente (Simulación).");
        }
    }

    public class RegisterRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EsAdmin { get; set; } = false; // Nuevo campo
    }


    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class GoogleLoginRequest
    {
        public string IdToken { get; set; } = string.Empty;
    }
}
