using Microsoft.EntityFrameworkCore;
using Migrations.Models;

namespace Migrations.Contexts
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}
