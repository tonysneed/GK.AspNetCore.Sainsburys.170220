﻿using Microsoft.EntityFrameworkCore;
using RepositoriesQueries.Models;

namespace RepositoriesQueries.Contexts
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}
