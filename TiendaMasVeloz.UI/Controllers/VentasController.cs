using Microsoft.AspNetCore.Mvc;
using TiendaMasVeloz.BLL.Contracts;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.UI.Controllers
{
    public class VentasController : Controller
    {
        private readonly IFacturaService _facturaService;
        private readonly IProductoService _productoService;
        private readonly IClienteService _clienteService;

        public VentasController(
            IFacturaService facturaService,
            IProductoService productoService,
            IClienteService clienteService)
        {
            _facturaService = facturaService;
            _productoService = productoService;
            _clienteService = clienteService;
        }

        public async Task<IActionResult> Index()
        {
            var facturas = await _facturaService.ObtenerTodosAsync();
            return View(facturas);
        }

        public async Task<IActionResult> NuevaVenta()
        {
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                var clientes = await _clienteService.ObtenerTodosAsync();
                var ventasRecientes = await _facturaService.ObtenerTodosAsync();
                
                ViewBag.Productos = productos.Where(p => p.Activo && p.Stock > 0).ToList();
                ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();
                ViewBag.VentasRecientes = ventasRecientes.OrderByDescending(v => v.Fecha).Take(10).ToList();
                
                return View(new FacturaDTO { Fecha = DateTime.Now });
            }
            catch (Exception ex)
            {
                // Log the error
                ViewBag.Productos = new List<ProductoDTO>();
                ViewBag.Clientes = new List<ClienteDTO>();
                ViewBag.VentasRecientes = new List<FacturaDTO>();
                ModelState.AddModelError("", "Error al cargar los datos: " + ex.Message);
                return View(new FacturaDTO { Fecha = DateTime.Now });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NuevaVenta(FacturaDTO factura)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _facturaService.CrearAsync(factura);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar la venta: " + ex.Message);
            }

            // Recargar los productos y clientes en caso de error
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                var clientes = await _clienteService.ObtenerTodosAsync();
                
                ViewBag.Productos = productos.Where(p => p.Activo && p.Stock > 0).ToList();
                ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();
            }
            catch
            {
                ViewBag.Productos = new List<ProductoDTO>();
                ViewBag.Clientes = new List<ClienteDTO>();
            }
            
            return View(factura);
        }

        public async Task<IActionResult> Details(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarProducto([FromBody] DetalleFacturaDTO detalle)
        {
            var producto = await _productoService.GetProductoByIdAsync(detalle.IdProducto);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }

            detalle.PrecioUnitario = producto.PrecioVenta;
            detalle.Subtotal = detalle.PrecioUnitario * detalle.Cantidad;

            return Json(new { 
                success = true, 
                precioUnitario = detalle.PrecioUnitario,
                subtotal = detalle.Subtotal,
                nombreProducto = producto.Nombre
            });
        }

        public async Task<IActionResult> Resumen()
        {
            var ventasRecientes = await _facturaService.ObtenerVentasRecientesAsync();
            return View(ventasRecientes);
        }

        [HttpGet]
        public async Task<IActionResult> ProductosMasVendidos()
        {
            var productos = await _facturaService.ObtenerProductosMasVendidosAsync();
            return PartialView("_ResumenProductosMasVendidos", productos);
        }

        [HttpGet]
        public async Task<IActionResult> VentasPorCategoria()
        {
            var ventas = await _facturaService.ObtenerVentasPorCategoriaAsync();
            return PartialView("_ResumenVentasPorCategoria", ventas);
        }

        [HttpGet]
        public async Task<IActionResult> MejoresVendedores()
        {
            var vendedores = await _facturaService.ObtenerMejoresVendedoresAsync();
            return PartialView("_ResumenMejoresVendedores", vendedores);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }

            var productos = await _productoService.GetAllProductosAsync();
            var clientes = await _clienteService.ObtenerTodosAsync();
            
            ViewBag.Productos = productos.Where(p => p.Activo).ToList();
            ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();
            
            return View(factura);
        }

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
                    TempData["Success"] = "La venta ha sido actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar la venta: " + ex.Message);
                }
            }

            var productos = await _productoService.GetAllProductosAsync();
            var clientes = await _clienteService.ObtenerTodosAsync();
            
            ViewBag.Productos = productos.Where(p => p.Activo).ToList();
            ViewBag.Clientes = clientes.Where(c => c.Activo).ToList();
            
            return View(factura);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await _facturaService.ObtenerPorIdAsync(id);
            if (factura == null)
            {
                return NotFound();
            }
            return View(factura);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // Primero obtenemos la factura para verificar que existe
                var factura = await _facturaService.ObtenerPorIdAsync(id);
                if (factura == null)
                {
                    TempData["Error"] = "La venta no existe.";
                    return RedirectToAction(nameof(Index));
                }

                // Intentamos eliminar la factura
                var result = await _facturaService.EliminarAsync(id);
                if (result)
                {
                    TempData["Success"] = "La venta ha sido eliminada correctamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar la venta.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar la venta: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 