using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CedulaNit { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100)]
        public string Apellido { get; set; }

        [StringLength(20)]
        public string Documento { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        public int Incentivos { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }

        public Cliente()
        {
            Apellido = string.Empty;
            Documento = string.Empty;
            Direccion = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Facturas = new List<Factura>();
            Incentivos = 0;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }
    }
} 