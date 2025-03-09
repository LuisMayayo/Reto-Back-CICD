using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceBackend.Models;

namespace EcommerceBackend.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> GetByIdAsync(int id);
        Task<Usuario> GetByEmailAsync(string email);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        // Agrega este método para obtener todos los usuarios
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario> GetByResetTokenAsync(string resetToken);
    }
}
