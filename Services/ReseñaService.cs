using EcommerceBackend.Models;
using EcommerceBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Services
{
    public class ReseñaService : IReseñaService
    {
        private readonly IReseñaRepository _reseñaRepository;

        public ReseñaService(IReseñaRepository reseñaRepository)
        {
            _reseñaRepository = reseñaRepository;
        }

        public async Task<List<Reseña>> GetByProductoIdAsync(int productoId)
        {
            return await _reseñaRepository.GetByProductoIdAsync(productoId);
        }

        public async Task<double> GetPromedioCalificacionAsync(int productoId)
        {
            return await _reseñaRepository.GetPromedioCalificacionAsync(productoId);
        }

        public async Task AddAsync(Reseña reseña)
        {
            await _reseñaRepository.AddAsync(reseña);
        }

        public async Task DeleteAsync(int id)
        {
            await _reseñaRepository.DeleteAsync(id);
        }
    }
}
