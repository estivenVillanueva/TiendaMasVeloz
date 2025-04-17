namespace TiendaMasVeloz.BLL.DTOs
{
    public class VendedorDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int CantidadVentas { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal PromedioVentas { get; set; }
    }
} 