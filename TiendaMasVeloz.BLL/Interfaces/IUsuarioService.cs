using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO?> GetUsuarioByIdAsync(int id);
        Task<IEnumerable<UsuarioDTO>> GetAllUsuariosAsync();
        Task<UsuarioDTO> CreateUsuarioAsync(UsuarioDTO usuarioDto);
        Task<UsuarioDTO?> UpdateUsuarioAsync(int id, UsuarioDTO usuarioDto);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<UsuarioDTO?> AuthenticateAsync(LoginDTO loginDto);
        Task<bool> ChangePasswordAsync(int id, string currentPassword, string newPassword);
        Task<bool> IsUserInRoleAsync(int id, string role);
        Task<IEnumerable<UsuarioDTO>> ObtenerPorRolAsync(string rol);
    }
}