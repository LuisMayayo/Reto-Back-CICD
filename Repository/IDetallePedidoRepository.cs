using EcommerceBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Repositories
{
    public interface IDetallePedidoRepository
    {
        Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task<DetallePedido> GetByIdAsync(int id);
        Task AddAsync(DetallePedido detallePedido);
        Task DeleteAsync(int id);
    }
}
