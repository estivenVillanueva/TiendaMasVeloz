using System.Collections.Generic;
using System.Threading.Tasks;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.DAL.Interfaces
{
    public interface IProductoRepository : IGenericRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosByProveedorAsync(int idProveedor);
        Task<IEnumerable<Producto>> GetProductosByCategoriaAsync(int idCategoria);
        Task<IEnumerable<Producto>> GetProductosBajoStockAsync(int stockMinimo);
        Task<bool> UpdateStockAsync(int id, int cantidad);
    }
} 