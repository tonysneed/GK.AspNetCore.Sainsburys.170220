using System.Collections.Generic;
using System.Threading.Tasks;
using RepositoriesQueries.Models;

namespace RepositoriesQueries.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
    }
}
