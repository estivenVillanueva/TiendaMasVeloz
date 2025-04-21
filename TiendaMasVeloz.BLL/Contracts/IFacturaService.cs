using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Contracts
{
    public interface IFacturaService
    {
        Task<IEnumerable<FacturaDTO>> ObtenerTodosAsync();
        Task<FacturaDTO?> ObtenerPorIdAsync(int id);
        Task<FacturaDTO> CrearAsync(FacturaDTO factura);
        Task<FacturaDTO?> ActualizarAsync(int id, FacturaDTO factura);
        Task<bool> AnularAsync(int id);
        Task<bool> EliminarAsync(int id);
        Task<IEnumerable<FacturaDTO>> ObtenerPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<IEnumerable<FacturaDTO>> GetAllFacturasAsync();
        Task<FacturaDTO?> GetFacturaByIdAsync(int id);
        Task<IEnumerable<FacturaDTO>> GetFacturasByClienteAsync(int clienteId);
        Task<IEnumerable<FacturaDTO>> GetFacturasByEmpleadoAsync(int empleadoId);
        Task<IEnumerable<FacturaDTO>> GetFacturasByFechaAsync(DateTime fecha);
        Task<IEnumerable<FacturaDTO>> GetFacturasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin);
        Task<decimal> GetTotalVentasPorEmpleadoAsync(int empleadoId, int mes, int año);
        Task<decimal> GetTotalVentasPorClienteAsync(int clienteId, int mes, int año);
        Task<FacturaDTO> CreateFacturaAsync(NuevaFacturaDTO facturaDto);
        Task<bool> AnularFacturaAsync(int id);
        Task<IEnumerable<FacturaDTO>> ObtenerVentasRecientesAsync();
        Task<IEnumerable<ProductoVendidoDTO>> ObtenerProductosMasVendidosAsync();
        Task<IEnumerable<VentaPorCategoriaDTO>> ObtenerVentasPorCategoriaAsync();
        Task<IEnumerable<VendedorDTO>> ObtenerMejoresVendedoresAsync();
    }
} 