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
    public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
    {
        public ProductoRepository(TiendaMasVelozContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetProductosByProveedorAsync(int idProveedor)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosByCategoriaAsync(int idCategoria)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Where(p => p.CategoriaId == idCategoria && p.Activo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosBajoStockAsync(int stockMinimo)
        {
            return await _context.Set<Producto>()
                .Include(p => p.Categoria)
                .Where(p => p.Stock <= stockMinimo && p.Activo)
                .ToListAsync();
        }

        public async Task<bool> UpdateStockAsync(int id, int cantidad)
        {
            var producto = await _context.Set<Producto>().FindAsync(id);
            if (producto == null)
                return false;

            producto.Stock += cantidad;
            if (producto.Stock < 0)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExisteProducto(string codigo)
        {
            return await _context.Set<Producto>()
                .AnyAsync(p => p.Codigo == codigo);
        }
    }
} 