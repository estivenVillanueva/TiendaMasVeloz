using System;
using System.Collections.Generic;

namespace TiendaMasVeloz.BLL.DTOs
{
    public class FacturaDTO
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreVendedor { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }
        public List<DetalleFacturaDTO> Detalles { get; set; } = new List<DetalleFacturaDTO>();
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
} 