using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackend.Repositories
{
    public class DetallePedidoRepository : IDetallePedidoRepository
    {
        private readonly EcommerceDbContext _context;

        public DetallePedidoRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtener detalles de un pedido por su ID.
        /// Incluye información del producto y la categoría.
        /// </summary>
        public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallesPedido // 🔹 Corregido (Plural)
                .Include(d => d.Producto)        // 🔹 Cargar información del producto
                    .ThenInclude(p => p.Categoria) // 🔹 Cargar categoría del producto
                .Where(d => d.PedidoId == pedidoId)
                .ToListAsync();
        }

        /// <summary>
        /// Obtener un detalle de pedido por su ID.
        /// </summary>
        public async Task<DetallePedido> GetByIdAsync(int id)
        {
            return await _context.DetallesPedido // 🔹 Corregido (Plural)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// Agregar un nuevo detalle de pedido.
        /// </summary>
        public async Task AddAsync(DetallePedido detallePedido)
        {
            _context.DetallesPedido.Add(detallePedido); // 🔹 Corregido (Plural)
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Eliminar un detalle de pedido por su ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var detalle = await _context.DetallesPedido.FindAsync(id); // 🔹 Corregido (Plural)
            if (detalle != null)
            {
                _context.DetallesPedido.Remove(detalle); // 🔹 Corregido (Plural)
                await _context.SaveChangesAsync();
            }
        }
    }
}
