using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using TiendaMasVeloz.DAL.Interfaces;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.BLL.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmpleadoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmpleadoDTO?> GetEmpleadoByIdAsync(int id)
        {
            var empleado = await _unitOfWork.Repository<Empleado>().GetByIdAsync(id);
            if (empleado == null) return null;
            
            var empleadoDto = _mapper.Map<EmpleadoDTO>(empleado);
            empleadoDto.IdEmpleado = empleado.Id;
            return empleadoDto;
        }

        public async Task<IEnumerable<EmpleadoDTO>> GetAllEmpleadosAsync()
        {
            try
            {
                var empleados = await _unitOfWork.Repository<Empleado>().GetAllAsync();
                var empleadosActivos = empleados
                    .Where(e => e.Activo)
                    .Select(e => new EmpleadoDTO
                    {
                        IdEmpleado = e.Id,
                        Cedula = e.Cedula ?? string.Empty,
                        Nombre = e.Nombre ?? string.Empty,
                        Apellido = e.Apellido ?? string.Empty,
                        Email = e.Email ?? string.Empty,
                        Telefono = e.Telefono ?? string.Empty,
                        Direccion = e.Direccion ?? string.Empty,
                        Cargo = e.Cargo ?? string.Empty,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion
                    })
                    .ToList();

                // Agregar logging para depuración
                System.Diagnostics.Debug.WriteLine($"Total de empleados encontrados: {empleados.Count()}");
                System.Diagnostics.Debug.WriteLine($"Empleados activos: {empleadosActivos.Count()}");
                foreach (var emp in empleadosActivos)
                {
                    System.Diagnostics.Debug.WriteLine($"Empleado: ID={emp.IdEmpleado}, Nombre={emp.Nombre} {emp.Apellido}");
                }

                return empleadosActivos;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en GetAllEmpleadosAsync: {ex.Message}");
                throw new Exception($"Error al obtener los empleados: {ex.Message}", ex);
            }
        }

        public async Task<EmpleadoDTO> CreateEmpleadoAsync(EmpleadoDTO empleadoDto)
        {
            var empleado = new Empleado
            {
                Cedula = empleadoDto.Cedula,
                Nombre = empleadoDto.Nombre,
                Apellido = empleadoDto.Apellido,
                Email = empleadoDto.Email,
                Telefono = empleadoDto.Telefono,
                Direccion = empleadoDto.Direccion,
                Cargo = empleadoDto.Cargo,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            await _unitOfWork.Repository<Empleado>().AddAsync(empleado);
            await _unitOfWork.CompleteAsync();

            empleadoDto.IdEmpleado = empleado.Id;
            return empleadoDto;
        }

        public async Task<EmpleadoDTO> UpdateEmpleadoAsync(int id, EmpleadoDTO empleadoDto)
        {
            var empleado = await _unitOfWork.Repository<Empleado>().GetByIdAsync(id);
            if (empleado == null)
                throw new Exception("Empleado no encontrado.");

            _mapper.Map(empleadoDto, empleado);
            _unitOfWork.Repository<Empleado>().Update(empleado);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<EmpleadoDTO>(empleado);
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            var empleado = await _unitOfWork.Repository<Empleado>().GetByIdAsync(id);
            if (empleado == null)
                return false;

            _unitOfWork.Repository<Empleado>().Remove(empleado);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<EmpleadoDTO> GetEmpleadoByCedulaAsync(string cedula)
        {
            var empleados = await _unitOfWork.Repository<Empleado>()
                .FindAsync(e => e.Cedula == cedula);
            return _mapper.Map<EmpleadoDTO>(empleados.FirstOrDefault());
        }

        public async Task<decimal> GetComisionesEmpleadoAsync(int id, int mes, int año)
        {
            var facturas = await _unitOfWork.Repository<Factura>()
                .FindAsync(f => f.IdEmpleado == id && 
                               f.Fecha.Month == mes && 
                               f.Fecha.Year == año);

            var totalVentas = facturas.Sum(f => f.Total);
            // Por ahora retornamos un porcentaje fijo del 5% como comisión
            return totalVentas * 0.05m;
        }
    }
} 