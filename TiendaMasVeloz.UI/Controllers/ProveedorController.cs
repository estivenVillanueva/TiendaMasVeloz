using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TiendaMasVeloz.BLL.DTOs;
using TiendaMasVeloz.BLL.Interfaces;

namespace TiendaMasVeloz.UI.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.GetAllProveedoresAsync();
            return View(proveedores);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorDTO proveedorDto)
        {
            if (ModelState.IsValid)
            {
                await _proveedorService.CreateProveedorAsync(proveedorDto);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedorDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var proveedor = await _proveedorService.GetProveedorByIdAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProveedorDTO proveedorDto)
        {
            if (id != proveedorDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _proveedorService.UpdateProveedorAsync(proveedorDto);
                return RedirectToAction(nameof(Index));
            }
            return View(proveedorDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _proveedorService.GetProveedorByIdAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _proveedorService.DeleteProveedorAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
} 