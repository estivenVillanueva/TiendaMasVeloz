using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TiendaMasVeloz.DAL.Context
{
    public class TiendaMasVelozContextFactory : IDesignTimeDbContextFactory<TiendaMasVelozContext>
    {
        public TiendaMasVelozContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TiendaMasVelozContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=TiendaMasVeloz;User=root;Password=;",
                new MySqlServerVersion(new Version(8, 0, 0))
            );

            return new TiendaMasVelozContext(optionsBuilder.Options);
        }
    }
} 