using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.BLL.DTOs
{
    public class ProveedorDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El contacto es requerido")]
        [StringLength(100, ErrorMessage = "El contacto no puede tener más de 100 caracteres")]
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es requerida")]
        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }
    }
} 