using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.BLL.DTOs
{
    public class NuevaFacturaDTO
    {
        [Required(ErrorMessage = "El ID del cliente es requerido")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El ID del empleado es requerido")]
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "Se requiere al menos un detalle de factura")]
        [MinLength(1, ErrorMessage = "Se requiere al menos un detalle de factura")]
        public List<NuevoDetalleFacturaDTO> Detalles { get; set; } = new List<NuevoDetalleFacturaDTO>();
    }

    public class NuevoDetalleFacturaDTO
    {
        [Required(ErrorMessage = "El ID del producto es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public int Cantidad { get; set; }
    }
} 