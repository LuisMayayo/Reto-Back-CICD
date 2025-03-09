using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;

namespace EcommerceBackend.Services
{
    public class JwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration config)
        {
            _key = config["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key no se encuentra en la configuración.");
            _issuer = config["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer no se encuentra en la configuración.");
            _audience = config["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience no se encuentra en la configuración.");
        }

        public string GenerateToken(int userId, string email, bool esAdmin)
        {
            var claims = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),  // ID del usuario
    new Claim(JwtRegisteredClaimNames.Email, email),  // Email del usuario
    new Claim("role", esAdmin ? "Admin" : "User"), // Claim de rol (nombre estándar)
    new Claim(ClaimTypes.Role, esAdmin ? "Admin" : "User"), // Claim alternativo
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Identificador único del token
};


            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var signingKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
