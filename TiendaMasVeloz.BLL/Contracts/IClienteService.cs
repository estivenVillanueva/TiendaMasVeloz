using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Contracts
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> ObtenerTodosAsync();
        Task<ClienteDTO> ObtenerPorIdAsync(int id);
        Task<ClienteDTO> CrearAsync(ClienteDTO cliente);
        Task<ClienteDTO> ActualizarAsync(ClienteDTO cliente);
        Task EliminarAsync(int id);
    }
} 