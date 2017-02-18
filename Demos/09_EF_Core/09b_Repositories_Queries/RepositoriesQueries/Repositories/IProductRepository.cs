using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoriesQueries.Models;

namespace RepositoriesQueries.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task LoadCategory(Product product);
    }
}
