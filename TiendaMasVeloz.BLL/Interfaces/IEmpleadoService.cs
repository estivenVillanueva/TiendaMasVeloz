using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Interfaces
{
    public interface IEmpleadoService
    {
        Task<EmpleadoDTO?> GetEmpleadoByIdAsync(int id);
        Task<IEnumerable<EmpleadoDTO>> GetAllEmpleadosAsync();
        Task<EmpleadoDTO> CreateEmpleadoAsync(EmpleadoDTO empleadoDto);
        Task<EmpleadoDTO> UpdateEmpleadoAsync(int id, EmpleadoDTO empleadoDto);
        Task<bool> DeleteEmpleadoAsync(int id);
        Task<EmpleadoDTO?> GetEmpleadoByCedulaAsync(string cedula);
        Task<decimal> GetComisionesEmpleadoAsync(int id, int mes, int año);
    }
} 