using EcommerceBackend.DTOs;
using EcommerceBackend.Models;

public static class UsuarioExtensions
{
    public static UsuarioDto ToDto(this Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            FechaRegistro = usuario.FechaRegistro,
            EsAdmin = usuario.EsAdmin
        };
    }
}
