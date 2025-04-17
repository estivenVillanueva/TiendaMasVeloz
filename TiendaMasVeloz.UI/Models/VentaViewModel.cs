using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.UI.Models
{
    public class VentaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        [Display(Name = "Fecha de Venta")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public List<DetalleVentaViewModel> Detalles { get; set; } = new List<DetalleVentaViewModel>();
    }

    public class DetalleVentaViewModel
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
} 