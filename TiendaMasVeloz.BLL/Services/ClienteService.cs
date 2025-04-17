using TiendaMasVeloz.BLL.Contracts;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.DAL.Context;
using TiendaMasVeloz.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace TiendaMasVeloz.BLL.Services
{
    public class ClienteService : IClienteService
    {
        private readonly TiendaMasVelozContext _context;

        public ClienteService(TiendaMasVelozContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClienteDTO>> ObtenerTodosAsync()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Select(c => new ClienteDTO
                {
                    Id = c.Id,
                    CedulaNit = c.CedulaNit,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido ?? string.Empty,
                    Documento = c.Documento ?? string.Empty,
                    Email = c.Email ?? string.Empty,
                    Telefono = c.Telefono ?? string.Empty,
                    Direccion = c.Direccion ?? string.Empty,
                    Incentivos = c.Incentivos,
                    Activo = c.Activo,
                    FechaCreacion = c.FechaCreacion
                })
                .ToListAsync();
            
            return clientes;
        }

        public async Task<ClienteDTO> ObtenerPorIdAsync(int id)
        {
            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
                throw new KeyNotFoundException($"No se encontró el cliente con ID {id}");

            return new ClienteDTO
            {
                Id = cliente.Id,
                CedulaNit = cliente.CedulaNit,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido ?? string.Empty,
                Documento = cliente.Documento ?? string.Empty,
                Email = cliente.Email ?? string.Empty,
                Telefono = cliente.Telefono ?? string.Empty,
                Direccion = cliente.Direccion ?? string.Empty,
                Incentivos = cliente.Incentivos,
                Activo = cliente.Activo,
                FechaCreacion = cliente.FechaCreacion
            };
        }

        public async Task<ClienteDTO> CrearAsync(ClienteDTO clienteDto)
        {
            var cliente = new Cliente
            {
                CedulaNit = clienteDto.CedulaNit,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Documento = clienteDto.Documento,
                Email = clienteDto.Email,
                Telefono = clienteDto.Telefono,
                Direccion = clienteDto.Direccion,
                Incentivos = clienteDto.Incentivos,
                Activo = clienteDto.Activo,
                FechaCreacion = DateTime.Now
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            clienteDto.Id = cliente.Id;
            return clienteDto;
        }

        public async Task<ClienteDTO> ActualizarAsync(ClienteDTO clienteDto)
        {
            var cliente = await _context.Clientes.FindAsync(clienteDto.Id);
            if (cliente == null)
                throw new KeyNotFoundException($"No se encontró el cliente con ID {clienteDto.Id}");

            cliente.CedulaNit = clienteDto.CedulaNit;
            cliente.Nombre = clienteDto.Nombre;
            cliente.Apellido = clienteDto.Apellido;
            cliente.Documento = clienteDto.Documento;
            cliente.Email = clienteDto.Email;
            cliente.Telefono = clienteDto.Telefono;
            cliente.Direccion = clienteDto.Direccion;
            cliente.Incentivos = clienteDto.Incentivos;
            cliente.Activo = clienteDto.Activo;

            await _context.SaveChangesAsync();
            return clienteDto;
        }

        public async Task EliminarAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
        }
    }
} 