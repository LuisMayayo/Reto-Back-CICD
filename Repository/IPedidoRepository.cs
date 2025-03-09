// IRepository/IPedidoRepository.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Repositories
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> GetByUserIdAsync(int userId);
        Task<Pedido> GetByIdAsync(int id);
        Task<Pedido> CreateAsync(Pedido pedido);
        Task DeleteAsync(int id);
        Task<Pedido> GetByIdWithUserAsync(int id);

         Task<List<Pedido>> GetAllAsync();
         Task<Pedido> GetByIdWithDetailsAsync(int id);
    }
}
