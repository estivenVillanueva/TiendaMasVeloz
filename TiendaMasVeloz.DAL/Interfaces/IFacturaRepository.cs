using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.DAL.Interfaces
{
    public interface IFacturaRepository : IGenericRepository<Factura>
    {
        Task<IEnumerable<Factura>> GetFacturasPorCliente(int clienteId);
        Task<IEnumerable<Factura>> GetFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin);
    }
} 