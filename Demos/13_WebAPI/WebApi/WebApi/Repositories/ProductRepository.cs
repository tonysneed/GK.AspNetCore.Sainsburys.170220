using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ProductRepository : IProductsRepository
    {
        private static readonly ConcurrentDictionary<int, Product> ProductsCache =
            new ConcurrentDictionary<int, Product>();

        static ProductRepository()
        {
            ProductsCache.TryAdd(1, new Product { Id = 1, ProductName = "Ristretto", Price = 10 });
            ProductsCache.TryAdd(2, new Product { Id = 2, ProductName = "Espresso", Price = 20 });
            ProductsCache.TryAdd(3, new Product { Id = 3, ProductName = "Macchiato", Price = 30 });
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(ProductsCache.Values);
        }

        public async Task<Product> GetProductAsync(int id)
        {
            Product product;
            if (!ProductsCache.TryGetValue(id, out product))
            {
                throw new KeyNotFoundException($"Product with id '{id}' does not exist in cache");
            }
            return await Task.FromResult(product);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await Task.FromResult(ProductsCache.TryAdd(product.Id, product));
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (!ProductsCache.ContainsKey(product.Id))
            {
                throw new KeyNotFoundException($"Product with id '{product.Id}' does not exist in cache");
            }
            await Task.FromResult(ProductsCache.TryUpdate(
                product.Id, product, ProductsCache[product.Id]));
            return product;
        }

        public async Task DeleteAsync(int id)
        {
            Product product;
            await Task.FromResult(ProductsCache.TryRemove(id, out product));
        }
    }
}
