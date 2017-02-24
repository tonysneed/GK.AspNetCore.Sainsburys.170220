using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAngularApp.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Authorize]
        [Route("secure")]
        public IActionResult Secure()
        {
            var claims = User.Claims.ToArray();
            return View(claims);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");
                await HttpContext.Authentication.SignOutAsync("oidc");
                return RedirectToAction("Logout");
            }
            return View("Index");
        }
    }
}
