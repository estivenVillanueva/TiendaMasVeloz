using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(50)]
        public string Cargo { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Empleado()
        {
            Cedula = string.Empty;
            Nombre = string.Empty;
            Apellido = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
            Direccion = string.Empty;
            Cargo = string.Empty;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }
    }
} 