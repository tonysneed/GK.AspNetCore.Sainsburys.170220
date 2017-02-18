using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using RepositoriesQueries.Models;
using RepositoriesQueries.Repositories;

namespace RepositoriesQueries.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductController(IProductRepository productRepo,
            ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _productRepo.GetProducts();
            return View(model);
        }

        // GET: /<controller>/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetProduct(id);
            var categories = await _categoryRepo.GetCategories();
            var vm = new EditProductViewModel(product, categories);
            return View(vm);
        }

        // POST: /<controller>/Edit
        [HttpPost]
        public async Task<IActionResult> Edit([Bind(Prefix = "Product")]Product model)
        {
            await _productRepo.UpdateProduct(model);
            return RedirectToAction("Index");
        }
    }
}