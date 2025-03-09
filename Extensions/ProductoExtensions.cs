using System;
using EcommerceBackend.Models;
using EcommerceBackend.DTOs;

namespace EcommerceBackend.Extensions
{
    public static class ProductoExtensions
    {
        public static void Validate(this Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto no puede estar vacío.");
            if (producto.Precio <= 0)
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            if (producto.CategoriaId <= 0)
                throw new ArgumentException("Debe seleccionar una categoría válida.");
        }

        public static ProductoDto ToDto(this Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                Descripcion = producto.Descripcion ?? string.Empty,
                UrlImagen = producto.UrlImagen ?? string.Empty,
                // Si la categoría está cargada, incluir su nombre
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty
            };
        }
    }
}
