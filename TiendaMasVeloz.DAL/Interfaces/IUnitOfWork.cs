using System;
using System.Threading.Tasks;

namespace TiendaMasVeloz.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        IProductoRepository ProductoRepository { get; }
        IFacturaRepository FacturaRepository { get; }
        Task<int> CompleteAsync();
    }
} 