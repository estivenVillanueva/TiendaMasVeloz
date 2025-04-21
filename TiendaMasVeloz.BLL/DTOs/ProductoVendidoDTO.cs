namespace TiendaMasVeloz.BLL.DTOs
{
    public class ProductoVendidoDTO
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVentas { get; set; }
    }
} 