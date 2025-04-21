using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.DAL.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }

        public Categoria()
        {
            Activo = true;
            FechaCreacion = DateTime.Now;
            Productos = new List<Producto>();
        }
    }
} 