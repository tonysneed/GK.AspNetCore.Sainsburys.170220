using Microsoft.AspNetCore.Mvc;
using HelloTagHelpers.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloTagHelpers.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var vm = new HomeViewModel {Name = "Developer"};
            return View(vm);
        }

        [Route("/goodbye")]
        public IActionResult SayGoodbye()
        {
            return Content("Goodbye!");
        }
    }
}
