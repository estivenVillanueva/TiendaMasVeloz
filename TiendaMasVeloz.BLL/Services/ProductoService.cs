using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using TiendaMasVeloz.DAL.Interfaces;
using TiendaMasVeloz.DAL.Entities;
using TiendaMasVeloz.BLL.Contracts;
using TiendaMasVeloz.DAL.Context;

namespace TiendaMasVeloz.BLL.Services
{
    public class ProductoService : IProductoService
    {
        private readonly TiendaMasVelozContext _context;

        public ProductoService(TiendaMasVelozContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllProductosAsync()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                IdProducto = p.Id,
                Nombre = p.Nombre ?? string.Empty,
                Descripcion = p.Descripcion ?? string.Empty,
                PrecioCosto = p.PrecioCosto,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
                Activo = p.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = null
            });
        }

        public async Task<ProductoDTO> GetProductoByIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                throw new KeyNotFoundException($"No se encontró el producto con ID {id}");

            return new ProductoDTO
            {
                IdProducto = producto.Id,
                Nombre = producto.Nombre ?? string.Empty,
                Descripcion = producto.Descripcion ?? string.Empty,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty,
                Activo = producto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = null
            };
        }

        public async Task<ProductoDTO> CreateProductoAsync(ProductoDTO productoDto)
        {
            var producto = new Producto
            {
                Nombre = productoDto.Nombre,
                Descripcion = productoDto.Descripcion,
                PrecioCosto = productoDto.PrecioCosto,
                PrecioVenta = productoDto.PrecioVenta,
                Stock = productoDto.Stock,
                CategoriaId = productoDto.CategoriaId,
                Activo = productoDto.Activo
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            productoDto.IdProducto = producto.Id;
            return productoDto;
        }

        public async Task<ProductoDTO> UpdateProductoAsync(int id, ProductoDTO productoDto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                throw new KeyNotFoundException($"No se encontró el producto con ID {id}");

            producto.Nombre = productoDto.Nombre;
            producto.Descripcion = productoDto.Descripcion;
            producto.PrecioCosto = productoDto.PrecioCosto;
            producto.PrecioVenta = productoDto.PrecioVenta;
            producto.Stock = productoDto.Stock;
            producto.CategoriaId = productoDto.CategoriaId;
            producto.Activo = productoDto.Activo;

            await _context.SaveChangesAsync();
            return productoDto;
        }

        public async Task DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UpdateStockAsync(int id, int cantidad)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            producto.Stock += cantidad;
            if (producto.Stock < 0)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProductoDetalleDTO?> GetProductoDetalleByIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
                return null;

            return new ProductoDetalleDTO
            {
                IdProducto = producto.Id,
                Nombre = producto.Nombre ?? string.Empty,
                Descripcion = producto.Descripcion ?? string.Empty,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty,
                Activo = producto.Activo,
                FechaCreacion = producto.FechaCreacion,
                FechaModificacion = null
            };
        }

        // Comentado temporalmente hasta que se agregue la columna en la base de datos
        /*
        public async Task<IEnumerable<ProductoDTO>> GetProductosByProveedorAsync(int idProveedor)
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.ProveedorId == idProveedor)
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                IdProducto = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                PrecioCosto = p.PrecioCosto,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
                Activo = p.Activo,
                FechaCreacion = p.FechaCreacion,
                FechaModificacion = null
            });
        }
        */

        public async Task<IEnumerable<ProductoDTO>> GetProductosBajoStockAsync(int stockMinimo)
        {
            var productos = await _context.Productos
                .Where(p => p.Stock <= stockMinimo)
                .Include(p => p.Categoria)
                .ToListAsync();

            return productos.Select(p => new ProductoDTO
            {
                IdProducto = p.Id,
                Nombre = p.Nombre ?? string.Empty,
                Descripcion = p.Descripcion ?? string.Empty,
                PrecioCosto = p.PrecioCosto,
                PrecioVenta = p.PrecioVenta,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria?.Nombre ?? string.Empty,
                Activo = p.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = null
            });
        }

        public async Task<ProductoDTO?> GetProductoByCodigoProveedorAsync(string codigoProveedor)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Codigo == codigoProveedor);

            if (producto == null) return null;

            return new ProductoDTO
            {
                IdProducto = producto.Id,
                Nombre = producto.Nombre ?? string.Empty,
                Descripcion = producto.Descripcion ?? string.Empty,
                PrecioCosto = producto.PrecioCosto,
                PrecioVenta = producto.PrecioVenta,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria?.Nombre ?? string.Empty,
                Activo = producto.Activo,
                FechaCreacion = DateTime.Now,
                FechaModificacion = null
            };
        }
    }
} 