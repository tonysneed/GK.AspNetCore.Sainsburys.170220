using Microsoft.AspNetCore.Mvc;
using RepositoriesQueries.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RepositoriesQueries.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var vm = new HomeViewModel {Name = "Developer"};
            return View(vm);
        }
    }
}
