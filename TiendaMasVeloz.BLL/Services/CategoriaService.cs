using TiendaMasVeloz.BLL.Contracts;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.DAL.Context;
using TiendaMasVeloz.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace TiendaMasVeloz.BLL.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly TiendaMasVelozContext _context;

        public CategoriaService(TiendaMasVelozContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaDTO>> ObtenerTodosAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            return categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre ?? string.Empty,
                Descripcion = c.Descripcion ?? string.Empty,
                Activo = c.Activo
            });
        }

        public async Task<CategoriaDTO> ObtenerPorIdAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new KeyNotFoundException($"No se encontró la categoría con ID {id}");

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre ?? string.Empty,
                Descripcion = categoria.Descripcion ?? string.Empty,
                Activo = categoria.Activo
            };
        }

        public async Task<CategoriaDTO> CrearAsync(CategoriaDTO categoriaDto)
        {
            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion,
                Activo = categoriaDto.Activo
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            categoriaDto.Id = categoria.Id;
            return categoriaDto;
        }

        public async Task<CategoriaDTO> ActualizarAsync(CategoriaDTO categoriaDto)
        {
            var categoria = await _context.Categorias.FindAsync(categoriaDto.Id);
            if (categoria == null)
                throw new KeyNotFoundException($"No se encontró la categoría con ID {categoriaDto.Id}");

            categoria.Nombre = categoriaDto.Nombre;
            categoria.Descripcion = categoriaDto.Descripcion;
            categoria.Activo = categoriaDto.Activo;

            await _context.SaveChangesAsync();
            return categoriaDto;
        }

        public async Task EliminarAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }
    }
} 