// IServices/IPedidoService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IPedidoService
    {
        Task<List<Pedido>> GetByUserIdAsync(int userId);
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> CreateAsync(Pedido pedido);
        Task DeleteAsync(int id);
        Task EnviarConfirmacionPedidoAsync(int pedidoId);
        Task<List<Pedido>> GetAllAsync();
    }
}
