using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Factura
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public DateTime Fecha { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal IVA { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public string Numero { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        [ForeignKey("IdEmpleado")]
        public virtual Empleado? Empleado { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente? Cliente { get; set; }

        public virtual ICollection<DetalleFactura> Detalles { get; set; }

        public Factura()
        {
            Numero = string.Empty;
            Estado = true;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
            Detalles = new List<DetalleFactura>();
        }
    }
} 