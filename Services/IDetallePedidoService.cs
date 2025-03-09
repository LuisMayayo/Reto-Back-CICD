using EcommerceBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceBackend.Services
{
    public interface IDetallePedidoService
    {
        Task<List<DetallePedido>> GetByPedidoIdAsync(int pedidoId);
        Task<DetallePedido> GetByIdAsync(int id);
        Task<DetallePedido> AddAsync(DetallePedido detallePedido);
        Task DeleteAsync(int id);
    }
}
 