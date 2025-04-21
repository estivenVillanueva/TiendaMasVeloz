using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using TiendaMasVeloz.DAL.Context;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.BLL.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly TiendaMasVelozContext _context;
        private readonly IMapper _mapper;

        public ProveedorService(TiendaMasVelozContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProveedorDTO>> GetAllProveedoresAsync()
        {
            var proveedores = await _context.Proveedores.ToListAsync();
            return _mapper.Map<IEnumerable<ProveedorDTO>>(proveedores);
        }

        public async Task<ProveedorDTO> GetProveedorByIdAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            return _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<ProveedorDTO> CreateProveedorAsync(ProveedorDTO proveedorDto)
        {
            var proveedor = _mapper.Map<Proveedor>(proveedorDto);
            proveedor.FechaCreacion = DateTime.Now;
            proveedor.Activo = true;

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<ProveedorDTO> UpdateProveedorAsync(ProveedorDTO proveedorDto)
        {
            var proveedor = await _context.Proveedores.FindAsync(proveedorDto.Id);
            if (proveedor == null)
                return null;

            _mapper.Map(proveedorDto, proveedor);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProveedorDTO>(proveedor);
        }

        public async Task<bool> DeleteProveedorAsync(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
                return false;

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 