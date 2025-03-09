using EcommerceBackend.Data;
using EcommerceBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackend.Repositories
{
    public class ReseñaRepository : IReseñaRepository
    {
        private readonly EcommerceDbContext _context;

        public ReseñaRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reseña>> GetByProductoIdAsync(int productoId)
        {
            return await _context.Reseñas
                .Where(r => r.ProductoId == productoId)
                .Include(r => r.Usuario) // Incluir usuario para mostrar su nombre
                .ToListAsync();
        }

        public async Task<double> GetPromedioCalificacionAsync(int productoId)
        {
            var calificaciones = await _context.Reseñas
                .Where(r => r.ProductoId == productoId)
                .Select(r => (double)r.Calificacion)
                .ToListAsync(); // 👈 Se obtiene la lista primero

            if (calificaciones.Count == 0)
            {
                return 0; // Si no hay calificaciones, devolvemos 0 en lugar de llamar a AverageAsync()
            }

            return calificaciones.Average(); // 👈 Se calcula el promedio de forma segura
        }



        public async Task AddAsync(Reseña reseña)
        {
            _context.Reseñas.Add(reseña);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña != null)
            {
                _context.Reseñas.Remove(reseña);
                await _context.SaveChangesAsync();
            }
        }
    }
}
