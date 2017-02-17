using System;
using Microsoft.AspNetCore.Mvc;
using FiltersLogging.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FiltersLogging.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var vm = new HomeViewModel {Name = "Developer"};
            return View(vm);
        }

        [Route("/throw")]
        public IActionResult ThrowError()
        {
            throw new Exception("Doh!");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
