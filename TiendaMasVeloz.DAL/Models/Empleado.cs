using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.DAL.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        public decimal Comision { get; set; } = 0;
    }
} 