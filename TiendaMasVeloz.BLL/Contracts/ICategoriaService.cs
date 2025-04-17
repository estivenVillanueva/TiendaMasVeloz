using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Contracts
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync();
        Task<CategoriaDTO> ObtenerPorIdAsync(int id);
        Task<CategoriaDTO> CrearAsync(CategoriaDTO categoria);
        Task<CategoriaDTO> ActualizarAsync(CategoriaDTO categoria);
        Task EliminarAsync(int id);
    }
} 