using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.Contracts;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace TiendaMasVeloz.UI.Controllers
{
    public class VendedorViewModel
    {
        public int IdEmpleado { get; set; }
        public string NombreCompleto { get; set; }
    }

    public class FacturasController : Controller
    {
        private readonly IFacturaService _facturaService;
        private readonly IEmpleadoService _empleadoService;
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IClienteService _clienteService;

        public FacturasController(
            IFacturaService facturaService,
            IEmpleadoService empleadoService,
            IProductoService productoService,
            IUsuarioService usuarioService,
            IClienteService clienteService)
        {
            _facturaService = facturaService;
            _empleadoService = empleadoService;
            _productoService = productoService;
            _usuarioService = usuarioService;
            _clienteService = clienteService;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
            var facturas = await _facturaService.ObtenerTodosAsync();
            return View(facturas);
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        // GET: Facturas/Create
        public async Task<IActionResult> Create()
        {
            await CargarListasCreate();
            return View(new FacturaDTO());
        }

        // POST: Facturas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacturaDTO factura)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Por favor, complete todos los campos requeridos.");
                await CargarListasCreate();
                return View(factura);
            }

            if (factura.Detalles == null || !factura.Detalles.Any())
            {
                ModelState.AddModelError("", "La factura debe tener al menos un producto.");
                await CargarListasCreate();
                return View(factura);
            }

            try
            {
                factura.Estado = true;
                factura.FechaCreacion = DateTime.Now;
                factura.Fecha = DateTime.Now;
                factura.NumeroFactura = $"FAC-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}";
                
                await _facturaService.CrearAsync(factura);
                TempData["SuccessMessage"] = "Factura creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la factura: " + ex.Message);
                await CargarListasCreate();
                return View(factura);
            }
            await CargarListasCreate();
            return View(factura);
        }

        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            var clientes = await _clienteService.ObtenerTodosAsync();
            var productos = await _productoService.GetAllProductosAsync();

            ViewBag.Empleados = new SelectList(empleados.Select(e => new { e.IdEmpleado, NombreCompleto = $"{e.Nombre} {e.Apellido}" }), "IdEmpleado", "NombreCompleto", factura.IdEmpleado);
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", factura.IdCliente);
            ViewBag.Productos = new SelectList(productos, "IdProducto", "Nombre");
            return View(factura);
        }

        // POST: Facturas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FacturaDTO factura)
        {
            if (id != factura.IdFactura)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _facturaService.ActualizarAsync(id, factura);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la factura: " + ex.Message);
                }
            }

            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            var clientes = await _clienteService.ObtenerTodosAsync();
            var productos = await _productoService.GetAllProductosAsync();

            ViewBag.Empleados = new SelectList(empleados.Select(e => new { e.IdEmpleado, NombreCompleto = $"{e.Nombre} {e.Apellido}" }), "IdEmpleado", "NombreCompleto", factura.IdEmpleado);
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", factura.IdCliente);
            ViewBag.Productos = new SelectList(productos, "IdProducto", "Nombre");
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _facturaService.AnularAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al anular la factura: " + ex.Message);
                return View(await _facturaService.ObtenerPorIdAsync(id));
            }
        }

        public async Task<IActionResult> ReporteVentas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue)
                fechaInicio = DateTime.Today.AddDays(-30);
            if (!fechaFin.HasValue)
                fechaFin = DateTime.Today;

            // Asegurarse de que fechaFin incluya todo el día
            fechaFin = fechaFin.Value.Date.AddDays(1).AddTicks(-1);

            var facturas = await _facturaService.ObtenerPorRangoFechasAsync(fechaInicio.Value, fechaFin.Value);

            // Filtrar y calcular las métricas
            var facturasActivas = facturas.Where(f => f.Estado).ToList();
            var facturasAnuladas = facturas.Where(f => !f.Estado).ToList();

            // Calcular productos vendidos sumando la cantidad de cada detalle
            var productosVendidos = facturasActivas
                .Where(f => f.Detalles != null)
                .SelectMany(f => f.Detalles)
                .Sum(d => d.Cantidad);

            ViewBag.TotalVentas = facturasActivas.Sum(f => f.Total);
            ViewBag.FacturasActivas = facturasActivas.Count();
            ViewBag.ProductosVendidos = productosVendidos;
            ViewBag.FacturasAnuladas = facturasAnuladas.Count();

            ViewBag.FechaInicio = fechaInicio.Value;
            ViewBag.FechaFin = fechaFin.Value;

            return View(facturas);
        }

        public async Task<IActionResult> DetalleFactura(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        private async Task CargarListasCreate()
        {
            try
            {
                var empleados = await _empleadoService.GetAllEmpleadosAsync();
                var clientes = await _clienteService.ObtenerTodosAsync();
                var productos = await _productoService.GetAllProductosAsync();

                // Filtrar vendedores activos
                var vendedoresActivos = empleados
                    .Where(e => e.Activo && 
                           !string.IsNullOrEmpty(e.Cargo) && 
                           e.Cargo.Trim().ToLower().Replace(" ", "").Contains("vendedor"))
                    .Select(e => new VendedorViewModel
                    {
                        IdEmpleado = e.IdEmpleado,
                        NombreCompleto = $"{e.Nombre} {e.Apellido}".Trim()
                    })
                    .OrderBy(v => v.NombreCompleto)
                    .ToList();

                if (!vendedoresActivos.Any())
                {
                    ModelState.AddModelError("", "No hay vendedores activos disponibles en el sistema.");
                }

                ViewBag.Empleados = new SelectList(vendedoresActivos, "IdEmpleado", "NombreCompleto");

                ViewBag.Clientes = new SelectList(clientes?.Where(c => c.Activo) ?? Enumerable.Empty<ClienteDTO>(), "Id", "Nombre");
                ViewBag.Productos = productos?.Where(p => p.Stock > 0).ToList() ?? new List<ProductoDTO>();
                ViewBag.EstadosFactura = new SelectList(new[] { "Pendiente", "Pagada", "Anulada" });
            }
            catch (Exception ex)
            {
                ViewBag.Empleados = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Clientes = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.Productos = new List<ProductoDTO>();
                ViewBag.EstadosFactura = new SelectList(Enumerable.Empty<string>());
                ModelState.AddModelError("", $"Error al cargar las listas: {ex.Message}");
            }
        }
    }
}