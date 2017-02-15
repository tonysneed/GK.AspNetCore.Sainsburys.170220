using HelloMvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelloMvc.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            //return Content("Index");

            ViewData["firstname"] = "John";
            ViewBag.lastname = "Doe";
            //return View();

            var model = new Customer
            {
                FirstName = "Jane",
                LastName = "Doe"
            };
            return View(model);
        }

        [Route("/hello-world")]
        public IActionResult SayHello() => Content("Hello World", "text/plain");
    }
}
