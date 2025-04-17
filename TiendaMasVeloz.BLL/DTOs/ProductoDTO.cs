using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaMasVeloz.BLL.DTOs
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class ProductoDetalleDTO : ProductoDTO
    {
        public string ContactoProveedor { get; set; } = string.Empty;
        public string TelefonoProveedor { get; set; } = string.Empty;
        public string DireccionProveedor { get; set; } = string.Empty;
    }
} 