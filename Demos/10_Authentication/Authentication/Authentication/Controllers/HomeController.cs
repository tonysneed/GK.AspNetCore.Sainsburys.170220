using Microsoft.AspNetCore.Mvc;
using Authentication.Models;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            string name = "Developer";
            if (User.Identity.IsAuthenticated)
            {
                name = User.Identity.Name;
            }
            var vm = new HomeViewModel { Name = name };
            return View(vm);
        }
    }
}
