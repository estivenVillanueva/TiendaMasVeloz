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
    public class FacturaService : IFacturaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly TiendaMasVelozContext _context;

        public FacturaService(IUnitOfWork unitOfWork, IMapper mapper, TiendaMasVelozContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<FacturaDTO>> ObtenerTodosAsync()
        {
            try
            {
                var query = from f in _context.Facturas
                           .Include(f => f.Cliente)
                           .Include(f => f.Empleado)
                           .Include(f => f.Detalles)
                           .ThenInclude(d => d.Producto)
                           select new FacturaDTO
                           {
                               IdFactura = f.Id,
                               NumeroFactura = f.Numero == null ? string.Empty : f.Numero,
                               Fecha = f.Fecha,
                               IdCliente = f.IdCliente,
                               NombreCliente = f.Cliente == null ? string.Empty :
                                   ((f.Cliente.Nombre == null ? string.Empty : f.Cliente.Nombre) + " " +
                                    (f.Cliente.Apellido == null ? string.Empty : f.Cliente.Apellido)).Trim(),
                               IdEmpleado = f.IdEmpleado,
                               NombreVendedor = f.Empleado == null ? string.Empty :
                                   (f.Empleado.Nombre == null ? string.Empty : f.Empleado.Nombre),
                               Subtotal = f.Subtotal,
                               IVA = f.IVA,
                               Total = f.Total,
                               Estado = f.Estado,
                               Detalles = f.Detalles.Select(d => new DetalleFacturaDTO
                               {
                                   IdDetalle = d.Id,
                                   IdFactura = d.FacturaId,
                                   IdProducto = d.ProductoId,
                                   NombreProducto = d.Producto == null ? string.Empty :
                                       (d.Producto.Nombre == null ? string.Empty : d.Producto.Nombre),
                                   Cantidad = d.Cantidad,
                                   PrecioUnitario = d.PrecioUnitario,
                                   Subtotal = d.Subtotal
                               }).ToList()
                           };

                return await query.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                // Loguear el error aquí si tienes un sistema de logging
                throw new Exception("Error al obtener las facturas: " + ex.Message, ex);
            }
        }

        public async Task<FacturaDTO> ObtenerPorIdAsync(int id)
        {
            try
            {
                var facturaDto = await _context.Facturas
                    .Include(f => f.Cliente)
                    .Include(f => f.Empleado)
                    .Include(f => f.Detalles)
                        .ThenInclude(d => d.Producto)
                    .Where(f => f.Id == id)
                    .Select(f => new FacturaDTO
                    {
                        IdFactura = f.Id,
                        NumeroFactura = f.Numero ?? string.Empty,
                        Fecha = f.Fecha,
                        IdCliente = f.IdCliente,
                        NombreCliente = f.Cliente != null 
                            ? (f.Cliente.Nombre ?? string.Empty) + " " + (f.Cliente.Apellido ?? string.Empty)
                            : string.Empty,
                        IdEmpleado = f.IdEmpleado,
                        NombreVendedor = f.Empleado != null 
                            ? f.Empleado.Nombre ?? string.Empty 
                            : string.Empty,
                        Subtotal = f.Subtotal,
                        IVA = f.IVA,
                        Total = f.Total,
                        Estado = f.Estado,
                        FechaCreacion = f.FechaCreacion,
                        FechaModificacion = f.FechaModificacion,
                        Detalles = f.Detalles.Select(d => new DetalleFacturaDTO
                        {
                            IdDetalle = d.Id,
                            IdFactura = d.FacturaId,
                            IdProducto = d.ProductoId,
                            NombreProducto = d.Producto != null ? d.Producto.Nombre ?? string.Empty : string.Empty,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Subtotal = d.Subtotal
                        }).ToList()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (facturaDto == null)
                    throw new KeyNotFoundException($"No se encontró la factura con ID {id}");

                return facturaDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la factura con ID {id}: " + ex.Message, ex);
            }
        }

        public async Task<FacturaDTO> CrearAsync(FacturaDTO facturaDto)
        {
            var factura = new DAL.Entities.Factura
            {
                Numero = facturaDto.NumeroFactura,
                Fecha = facturaDto.Fecha,
                IdCliente = facturaDto.IdCliente,
                IdEmpleado = facturaDto.IdEmpleado,
                Subtotal = facturaDto.Subtotal,
                IVA = facturaDto.IVA,
                Total = facturaDto.Total,
                Estado = facturaDto.Estado,
                Detalles = facturaDto.Detalles.Select(d => new DAL.Entities.DetalleFactura
                {
                    ProductoId = d.IdProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            // Actualizar el stock de los productos
            foreach (var detalle in factura.Detalles)
            {
                var producto = await _context.Productos.FindAsync(detalle.ProductoId);
                if (producto != null)
                {
                    producto.Stock -= detalle.Cantidad;
                }
            }
            await _context.SaveChangesAsync();

            facturaDto.IdFactura = factura.Id;
            return facturaDto;
        }

        public async Task<IEnumerable<FacturaDTO>> ObtenerVentasRecientesAsync()
        {
            var fechaInicio = DateTime.Now.AddDays(-30);
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Empleado)
                .Where(f => f.Fecha >= fechaInicio)
                .OrderByDescending(f => f.Fecha)
                .Take(10)
                .ToListAsync();

            return facturas.Select(f => new FacturaDTO
            {
                IdFactura = f.Id,
                NumeroFactura = f.Numero ?? string.Empty,
                Fecha = f.Fecha,
                NombreCliente = f.Cliente != null ? $"{f.Cliente.Nombre ?? string.Empty} {f.Cliente.Apellido ?? string.Empty}" : string.Empty,
                NombreVendedor = f.Empleado?.Nombre ?? string.Empty,
                Total = f.Total,
                Estado = f.Estado
            });
        }

        public async Task<IEnumerable<ProductoVendidoDTO>> ObtenerProductosMasVendidosAsync()
        {
            var productos = await _context.DetalleFacturas
                .Include(d => d.Producto)
                .Where(d => d.Producto != null)
                .GroupBy(d => new { 
                    ProductoId = d.ProductoId, 
                    ProductoNombre = d.Producto != null ? (d.Producto.Nombre ?? string.Empty) : string.Empty 
                })
                .Select(g => new ProductoVendidoDTO
                {
                    ProductoId = g.Key.ProductoId,
                    ProductoNombre = g.Key.ProductoNombre,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVentas = g.Sum(d => d.Subtotal)
                })
                .OrderByDescending(p => p.CantidadVendida)
                .Take(5)
                .ToListAsync() ?? new List<ProductoVendidoDTO>();

            return productos;
        }

        public async Task<IEnumerable<VentaPorCategoriaDTO>> ObtenerVentasPorCategoriaAsync()
        {
            var ventas = await _context.DetalleFacturas
                .Include(d => d.Producto)
                .ThenInclude(p => p.Categoria)
                .Where(d => d.Producto != null && d.Producto.Categoria != null)
                .GroupBy(d => new { 
                    CategoriaId = d.Producto.CategoriaId, 
                    CategoriaNombre = d.Producto.Categoria != null ? (d.Producto.Categoria.Nombre ?? string.Empty) : string.Empty 
                })
                .Select(g => new VentaPorCategoriaDTO
                {
                    Categoria = g.Key.CategoriaNombre,
                    CantidadVendida = g.Sum(d => d.Cantidad),
                    TotalVentas = g.Sum(d => d.Subtotal),
                    CantidadProductos = g.Select(d => d.ProductoId).Distinct().Count()
                })
                .OrderByDescending(v => v.TotalVentas)
                .ToListAsync() ?? new List<VentaPorCategoriaDTO>();

            return ventas;
        }

        public async Task<IEnumerable<VendedorDTO>> ObtenerMejoresVendedoresAsync()
        {
            var vendedores = await _context.Facturas
                .Include(f => f.Empleado)
                .Where(f => f.Fecha >= DateTime.Now.AddMonths(-1) && f.Empleado != null)
                .GroupBy(f => new { 
                    IdEmpleado = f.IdEmpleado, 
                    Nombre = f.Empleado != null ? (f.Empleado.Nombre ?? string.Empty) : string.Empty 
                })
                .Select(g => new VendedorDTO
                {
                    Id = g.Key.IdEmpleado,
                    Nombre = g.Key.Nombre,
                    CantidadVentas = g.Count(),
                    TotalVentas = g.Sum(f => f.Total),
                    PromedioVentas = g.Average(f => f.Total)
                })
                .OrderByDescending(v => v.TotalVentas)
                .Take(5)
                .ToListAsync();

            return vendedores;
        }

        public async Task<bool> AnularAsync(int id)
        {
            var factura = await _unitOfWork.FacturaRepository.GetByIdAsync(id);

            if (factura == null)
            {
                throw new Exception("La factura no existe.");
            }

            if (!factura.Estado)
            {
                throw new Exception("La factura ya está anulada.");
            }

            // Devolver stock a los productos
            foreach (var detalle in factura.Detalles)
            {
                var producto = await _unitOfWork.ProductoRepository.GetByIdAsync(detalle.ProductoId);
                producto.Stock += detalle.Cantidad;
                _unitOfWork.ProductoRepository.Update(producto);
            }

            factura.Estado = false;
            factura.FechaModificacion = DateTime.Now;

            _unitOfWork.FacturaRepository.Update(factura);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<FacturaDTO>> ObtenerPorRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var query = from f in _context.Facturas
                           .Include(f => f.Cliente)
                           .Include(f => f.Empleado)
                           .Include(f => f.Detalles)
                               .ThenInclude(d => d.Producto)
                           where f.Fecha >= fechaInicio && f.Fecha <= fechaFin
                           select new FacturaDTO
                           {
                               IdFactura = f.Id,
                               NumeroFactura = f.Numero ?? string.Empty,
                               Fecha = f.Fecha,
                               IdCliente = f.IdCliente,
                               NombreCliente = f.Cliente != null 
                                   ? (f.Cliente.Nombre ?? string.Empty) + " " + (f.Cliente.Apellido ?? string.Empty)
                                   : string.Empty,
                               IdEmpleado = f.IdEmpleado,
                               NombreVendedor = f.Empleado != null 
                                   ? f.Empleado.Nombre ?? string.Empty 
                                   : string.Empty,
                               Subtotal = f.Subtotal,
                               IVA = f.IVA,
                               Total = f.Total,
                               Estado = f.Estado,
                               Detalles = f.Detalles.Select(d => new DetalleFacturaDTO
                               {
                                   IdDetalle = d.Id,
                                   IdFactura = d.FacturaId,
                                   IdProducto = d.ProductoId,
                                   NombreProducto = d.Producto != null ? d.Producto.Nombre ?? string.Empty : string.Empty,
                                   Cantidad = d.Cantidad,
                                   PrecioUnitario = d.PrecioUnitario,
                                   Subtotal = d.Subtotal
                               }).ToList()
                           };

                return await query.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las facturas por rango de fechas: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<FacturaDTO>> GetAllFacturasAsync()
        {
            var facturas = await _unitOfWork.Repository<Factura>().GetAllAsync();
            return _mapper.Map<IEnumerable<FacturaDTO>>(facturas);
        }

        public async Task<FacturaDTO> GetFacturaByIdAsync(int id)
        {
            var factura = await _unitOfWork.Repository<Factura>().GetByIdAsync(id);
            return _mapper.Map<FacturaDTO>(factura);
        }

        public async Task<IEnumerable<FacturaDTO>> GetFacturasByClienteAsync(int clienteId)
        {
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Empleado)
                .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
                .Where(f => f.IdCliente == clienteId)
                .ToListAsync();

            return facturas.Select(f => new FacturaDTO
            {
                IdFactura = f.Id,
                NumeroFactura = f.Numero ?? string.Empty,
                Fecha = f.Fecha,
                IdCliente = f.IdCliente,
                NombreCliente = f.Cliente != null ? $"{f.Cliente.Nombre ?? string.Empty} {f.Cliente.Apellido ?? string.Empty}" : string.Empty,
                IdEmpleado = f.IdEmpleado,
                NombreVendedor = f.Empleado?.Nombre ?? string.Empty,
                Subtotal = f.Subtotal,
                IVA = f.IVA,
                Total = f.Total,
                Estado = f.Estado,
                Detalles = f.Detalles?.Select(d => new DetalleFacturaDTO
                {
                    IdDetalle = d.Id,
                    IdFactura = d.FacturaId,
                    IdProducto = d.ProductoId,
                    NombreProducto = d.Producto?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                })?.ToList() ?? new List<DetalleFacturaDTO>()
            });
        }

        public async Task<IEnumerable<FacturaDTO>> GetFacturasByEmpleadoAsync(int empleadoId)
        {
            var facturas = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.Empleado)
                .Include(f => f.Detalles)
                .ThenInclude(d => d.Producto)
                .Where(f => f.IdEmpleado == empleadoId)
                .ToListAsync();

            return facturas.Select(f => new FacturaDTO
            {
                IdFactura = f.Id,
                NumeroFactura = f.Numero ?? string.Empty,
                Fecha = f.Fecha,
                IdCliente = f.IdCliente,
                NombreCliente = f.Cliente != null ? $"{f.Cliente.Nombre ?? string.Empty} {f.Cliente.Apellido ?? string.Empty}" : string.Empty,
                IdEmpleado = f.IdEmpleado,
                NombreVendedor = f.Empleado?.Nombre ?? string.Empty,
                Subtotal = f.Subtotal,
                IVA = f.IVA,
                Total = f.Total,
                Estado = f.Estado,
                Detalles = f.Detalles?.Select(d => new DetalleFacturaDTO
                {
                    IdDetalle = d.Id,
                    IdFactura = d.FacturaId,
                    IdProducto = d.ProductoId,
                    NombreProducto = d.Producto?.Nombre ?? string.Empty,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                })?.ToList() ?? new List<DetalleFacturaDTO>()
            });
        }

        public async Task<IEnumerable<FacturaDTO>> GetFacturasByFechaAsync(DateTime fecha)
        {
            var facturas = await _unitOfWork.Repository<Factura>()
                .FindAsync(f => f.Fecha.Date == fecha.Date);
            return _mapper.Map<IEnumerable<FacturaDTO>>(facturas);
        }

        public async Task<IEnumerable<FacturaDTO>> GetFacturasByRangoFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var facturas = await _unitOfWork.Repository<Factura>()
                .FindAsync(f => f.Fecha.Date >= fechaInicio.Date && f.Fecha.Date <= fechaFin.Date);
            return _mapper.Map<IEnumerable<FacturaDTO>>(facturas);
        }

        public async Task<decimal> GetTotalVentasPorEmpleadoAsync(int empleadoId, int mes, int año)
        {
            var facturas = await _unitOfWork.Repository<Factura>()
                .FindAsync(f => f.IdEmpleado == empleadoId && 
                               f.Fecha.Month == mes && 
                               f.Fecha.Year == año);
            return facturas.Sum(f => f.Total);
        }

        public async Task<decimal> GetTotalVentasPorClienteAsync(int clienteId, int mes, int año)
        {
            var facturas = await _unitOfWork.Repository<Factura>()
                .FindAsync(f => f.IdCliente == clienteId && 
                               f.Fecha.Month == mes && 
                               f.Fecha.Year == año);
            return facturas.Sum(f => f.Total);
        }

        public async Task<FacturaDTO> CreateFacturaAsync(NuevaFacturaDTO facturaDto)
        {
            // Validar que existan los productos y tengan stock suficiente
            foreach (var detalle in facturaDto.Detalles)
            {
                var producto = await _unitOfWork.Repository<Producto>().GetByIdAsync(detalle.ProductoId);
                if (producto == null)
                {
                    throw new Exception($"El producto con ID {detalle.ProductoId} no existe.");
                }
                if (producto.Stock < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto {producto.Nombre}.");
                }
            }

            // Crear la factura
            var factura = new Factura
            {
                IdEmpleado = facturaDto.IdEmpleado,
                IdCliente = facturaDto.IdCliente,
                Fecha = DateTime.Now,
                Detalles = new List<DetalleFactura>()
            };

            // Agregar detalles y calcular totales
            decimal subtotal = 0;
            foreach (var detalleDto in facturaDto.Detalles)
            {
                var producto = await _unitOfWork.Repository<Producto>().GetByIdAsync(detalleDto.ProductoId);
                var detalle = new DetalleFactura
                {
                    ProductoId = detalleDto.ProductoId,
                    Cantidad = detalleDto.Cantidad,
                    PrecioUnitario = producto.PrecioVenta,
                    Subtotal = producto.PrecioVenta * detalleDto.Cantidad
                };
                factura.Detalles.Add(detalle);
                subtotal += detalle.Subtotal;
                
                // Actualizar stock
                producto.Stock -= detalleDto.Cantidad;
                _unitOfWork.Repository<Producto>().Update(producto);
            }

            // Calcular totales finales
            factura.Subtotal = subtotal;
            factura.IVA = subtotal * 0.19m;
            factura.Total = factura.Subtotal + factura.IVA;

            await _unitOfWork.Repository<Factura>().AddAsync(factura);
            await _unitOfWork.CompleteAsync();

            return await GetFacturaByIdAsync(factura.Id);
        }

        public async Task<bool> AnularFacturaAsync(int id)
        {
            var factura = await _unitOfWork.Repository<Factura>().GetByIdAsync(id);

            if (factura == null)
            {
                throw new Exception("La factura no existe.");
            }

            // Devolver stock a los productos
            foreach (var detalle in factura.Detalles)
            {
                var producto = await _unitOfWork.Repository<Producto>().GetByIdAsync(detalle.ProductoId);
                producto.Stock += detalle.Cantidad;
                _unitOfWork.Repository<Producto>().Update(producto);
            }

            _unitOfWork.Repository<Factura>().Remove(factura);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<FacturaDTO> ActualizarAsync(int id, FacturaDTO facturaDTO)
        {
            var factura = await _unitOfWork.FacturaRepository.GetByIdAsync(id);
            if (factura == null)
            {
                throw new Exception($"La factura con ID {id} no existe.");
            }

            // Validar que existan los productos y tengan stock suficiente
            foreach (var detalle in facturaDTO.Detalles)
            {
                var producto = await _unitOfWork.ProductoRepository.GetByIdAsync(detalle.IdProducto);
                if (producto == null)
                {
                    throw new Exception($"El producto con ID {detalle.IdProducto} no existe.");
                }

                // Calcular la diferencia de stock necesaria
                var detalleExistente = factura.Detalles.FirstOrDefault(d => d.ProductoId == detalle.IdProducto);
                var cantidadAnterior = detalleExistente?.Cantidad ?? 0;
                var diferenciaCantidad = detalle.Cantidad - cantidadAnterior;

                if (producto.Stock < diferenciaCantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto {producto.Nombre}.");
                }
            }

            // Actualizar stock de productos
            foreach (var detalle in facturaDTO.Detalles)
            {
                var producto = await _unitOfWork.ProductoRepository.GetByIdAsync(detalle.IdProducto);
                var detalleExistente = factura.Detalles.FirstOrDefault(d => d.ProductoId == detalle.IdProducto);
                var cantidadAnterior = detalleExistente?.Cantidad ?? 0;
                var diferenciaCantidad = detalle.Cantidad - cantidadAnterior;

                producto.Stock -= diferenciaCantidad;
                _unitOfWork.ProductoRepository.Update(producto);
            }

            // Actualizar la factura
            _mapper.Map(facturaDTO, factura);
            factura.FechaModificacion = DateTime.Now;

            // Recalcular totales
            factura.Subtotal = factura.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);
            factura.IVA = factura.Subtotal * 0.19m;
            factura.Total = factura.Subtotal + factura.IVA;

            _unitOfWork.FacturaRepository.Update(factura);
            await _unitOfWork.CompleteAsync();

            return await ObtenerPorIdAsync(id);
        }
    }
}