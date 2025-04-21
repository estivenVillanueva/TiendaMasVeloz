using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaMasVeloz.DAL.Entities
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioCosto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }

        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        // Comentado temporalmente hasta que se agregue la columna en la base de datos
        //public int ProveedorId { get; set; }

        //[ForeignKey("ProveedorId")]
        //public virtual Proveedor? Proveedor { get; set; }

        public bool Activo { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Producto()
        {
            PrecioCosto = 0;
            PrecioVenta = 0;
            Stock = 0;
            Activo = true;
            FechaCreacion = DateTime.Now;
        }
    }
} 