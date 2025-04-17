using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.BLL.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cédula/NIT es requerida")]
        [StringLength(20, ErrorMessage = "La cédula/NIT no puede tener más de 20 caracteres")]
        public string CedulaNit { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El apellido no puede tener más de 100 caracteres")]
        public string Apellido { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El documento no puede tener más de 20 caracteres")]
        public string Documento { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres")]
        public string Direccion { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido")]
        [StringLength(50, ErrorMessage = "El correo electrónico no puede tener más de 50 caracteres")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres")]
        public string Telefono { get; set; } = string.Empty;

        public int Incentivos { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
    }
} 