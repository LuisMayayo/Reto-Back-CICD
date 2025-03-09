using EcommerceBackend.Models;
using EcommerceBackend.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Services
{
    public class DetallePedidoService : IDetallePedidoService
    {
        private readonly IDetallePedidoRepository _detallePedidoRepository;

        public DetallePedidoService(IDetallePedidoRepository detallePedidoRepository)
        {
            _detallePedidoRepository = detallePedidoRepository;
        }

        public async Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _detallePedidoRepository.GetByPedidoIdAsync(pedidoId);
        }

        public async Task<DetallePedido> GetByIdAsync(int id)
        {
            return await _detallePedidoRepository.GetByIdAsync(id);
        }

        public async Task<DetallePedido> AddAsync(DetallePedido detallePedido)
        {
            await _detallePedidoRepository.AddAsync(detallePedido);
            return detallePedido;
        }

        public async Task DeleteAsync(int id)
        {
            await _detallePedidoRepository.DeleteAsync(id);
        }
    }
}
