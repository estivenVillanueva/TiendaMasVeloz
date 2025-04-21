using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TiendaMasVeloz.DAL.Context;
using TiendaMasVeloz.DAL.Entities;
using TiendaMasVeloz.DAL.Interfaces;

namespace TiendaMasVeloz.DAL.Repositories
{
    public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
    {
        public FacturaRepository(TiendaMasVelozContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorCliente(int clienteId)
        {
            return await _context.Set<Factura>()
                .Where(f => f.IdCliente == clienteId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Factura>> GetFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<Factura>()
                .Where(f => f.Fecha >= fechaInicio && f.Fecha <= fechaFin)
                .ToListAsync();
        }
    }
} 