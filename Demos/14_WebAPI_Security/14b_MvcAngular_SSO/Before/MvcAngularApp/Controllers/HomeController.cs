using Microsoft.AspNetCore.Mvc;

namespace MvcAngularApp.Controllers
{
    // TODO: Secure the web app
    // [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
