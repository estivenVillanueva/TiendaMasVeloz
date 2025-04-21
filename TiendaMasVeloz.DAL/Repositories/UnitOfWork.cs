using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TiendaMasVeloz.DAL.Context;
using TiendaMasVeloz.DAL.Interfaces;

namespace TiendaMasVeloz.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TiendaMasVelozContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;
        private IProductoRepository _productoRepository = null!;
        private IFacturaRepository _facturaRepository = null!;

        public UnitOfWork(TiendaMasVelozContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IGenericRepository<T>)_repositories[typeof(T)];
            }

            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IProductoRepository ProductoRepository
        {
            get
            {
                return _productoRepository ??= new ProductoRepository(_context);
            }
        }

        public IFacturaRepository FacturaRepository
        {
            get
            {
                return _facturaRepository ??= new FacturaRepository(_context);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
} 