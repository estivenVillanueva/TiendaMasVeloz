using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorDTO>> GetAllProveedoresAsync();
        Task<ProveedorDTO> GetProveedorByIdAsync(int id);
        Task<ProveedorDTO> CreateProveedorAsync(ProveedorDTO proveedorDto);
        Task<ProveedorDTO> UpdateProveedorAsync(ProveedorDTO proveedorDto);
        Task<bool> DeleteProveedorAsync(int id);
    }
} 