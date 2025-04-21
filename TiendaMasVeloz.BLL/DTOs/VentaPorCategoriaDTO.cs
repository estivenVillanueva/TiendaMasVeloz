namespace TiendaMasVeloz.BLL.DTOs
{
    public class VentaPorCategoriaDTO
    {
        public string Categoria { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVentas { get; set; }
        public int CantidadProductos { get; set; }
    }
} 