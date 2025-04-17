using Microsoft.EntityFrameworkCore;
using TiendaMasVeloz.DAL.Entities;

namespace TiendaMasVeloz.DAL.Context
{
    public class TiendaMasVelozContext : DbContext
    {
        public TiendaMasVelozContext(DbContextOptions<TiendaMasVelozContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.Property(e => e.FechaModificacion)
                    .HasColumnType("datetime(6)")
                    .IsRequired(false);
            });

            modelBuilder.Entity<Empleado>().ToTable("Empleados");
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<Proveedor>().ToTable("Proveedores");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Producto>().ToTable("Productos");
            modelBuilder.Entity<Factura>().ToTable("Facturas");
            modelBuilder.Entity<DetalleFactura>().ToTable("DetalleFacturas");
        }
    }
} 