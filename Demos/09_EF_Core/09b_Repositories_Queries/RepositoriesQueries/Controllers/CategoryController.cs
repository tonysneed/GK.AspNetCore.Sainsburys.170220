using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RepositoriesQueries.Repositories;

namespace RepositoriesQueries.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var model = await _categoryRepo.GetCategories();
            return View(model);
        }
    }
}