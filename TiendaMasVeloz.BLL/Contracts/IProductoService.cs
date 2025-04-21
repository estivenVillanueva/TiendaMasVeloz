using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.BLL.DTOs;

namespace TiendaMasVeloz.BLL.Contracts
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> GetAllProductosAsync();
        Task<ProductoDTO?> GetProductoByIdAsync(int id);
        Task<ProductoDTO> CreateProductoAsync(ProductoDTO producto);
        Task<ProductoDTO> UpdateProductoAsync(int id, ProductoDTO producto);
        Task DeleteProductoAsync(int id);
        Task<bool> UpdateStockAsync(int id, int cantidad);
        Task<ProductoDetalleDTO?> GetProductoDetalleByIdAsync(int id);
        // Comentado temporalmente hasta que se agregue la columna en la base de datos
        //Task<IEnumerable<ProductoDTO>> GetProductosByProveedorAsync(int idProveedor);
    }
} 