using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Contacto { get; set; }

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(200)]
        public string Direccion { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Proveedor()
        {
            Nombre = string.Empty;
            Contacto = string.Empty;
            Telefono = string.Empty;
            Direccion = string.Empty;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }
    }
} 