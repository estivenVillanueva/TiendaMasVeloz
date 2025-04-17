using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Contraseña { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Rol { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }
        
        public DateTime? FechaModificacion { get; set; }

        public Usuario()
        {
            FechaCreacion = DateTime.Now;
        }
    }
} 