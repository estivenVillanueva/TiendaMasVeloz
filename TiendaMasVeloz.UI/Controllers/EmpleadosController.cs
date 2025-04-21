using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;
using TiendaMasVeloz.BLL.Contracts;

namespace TiendaMasVeloz.UI.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IEmpleadoService _empleadoService;
        private readonly IFacturaService _facturaService;

        public EmpleadosController(IEmpleadoService empleadoService, IFacturaService facturaService)
        {
            _empleadoService = empleadoService;
            _facturaService = facturaService;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            var empleados = await _empleadoService.GetAllEmpleadosAsync();
            return View(empleados);
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpleadoDTO empleado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Si no se especifica un cargo, establecer como "Vendedor" por defecto
                    if (string.IsNullOrEmpty(empleado.Cargo))
                    {
                        empleado.Cargo = "Vendedor";
                    }

                    // Asegurarse de que el empleado se registre como activo
                    empleado.Activo = true;
                    empleado.FechaCreacion = DateTime.Now;

                    // Agregar logging para depuración
                    System.Diagnostics.Debug.WriteLine($"Creando empleado: Nombre={empleado.Nombre} {empleado.Apellido}, Cargo={empleado.Cargo}, Activo={empleado.Activo}");

                    var empleadoCreado = await _empleadoService.CreateEmpleadoAsync(empleado);
                    System.Diagnostics.Debug.WriteLine($"Empleado creado con ID: {empleadoCreado.IdEmpleado}");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error al crear empleado: {ex.Message}");
                    ModelState.AddModelError("", $"Error al crear el empleado: {ex.Message}");
                }
            }
            return View(empleado);
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmpleadoDTO empleado)
        {
            if (id != empleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _empleadoService.UpdateEmpleadoAsync(id, empleado);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
        }

        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _empleadoService.DeleteEmpleadoAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Si el empleado tiene facturas asociadas, en lugar de eliminarlo lo marcamos como inactivo
                var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
                if (empleado != null)
                {
                    empleado.Activo = false;
                    await _empleadoService.UpdateEmpleadoAsync(id, empleado);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Empleados/Comisiones
        public async Task<IActionResult> Comisiones(int id)
        {
            var empleado = await _empleadoService.GetEmpleadoByIdAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // GET: Empleados/GetComisiones
        [HttpGet]
        public async Task<IActionResult> GetComisiones(int id, int mes, int año)
        {
            try
            {
                // Obtener el total de ventas del empleado para el mes y año especificados
                var totalVentas = await _facturaService.GetTotalVentasPorEmpleadoAsync(id, mes, año);
                
                // Calcular la comisión (5% del total de ventas)
                var comision = totalVentas * 0.05m;

                return Json(new { totalVentas, comision });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 