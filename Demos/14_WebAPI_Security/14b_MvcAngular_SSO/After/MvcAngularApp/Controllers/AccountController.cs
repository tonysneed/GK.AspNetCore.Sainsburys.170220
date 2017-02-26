using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MvcAngularApp.Controllers
{
    [Authorize]
    public class AcountController : Controller
    {
        // GET: /username
        [HttpGet("/[action]")]
        public IActionResult Username()
        {
            string username = "";
            if (User.Identity.IsAuthenticated)
            {
                username = User.FindFirst("name").Value;
            }
            return Json(username);
        }

        // GET: /claims
        [HttpGet("/[action]")]
        public IActionResult Claims()
        {
            var claims = from c in User.Claims select new { c.Type, c.Value };
            return Json(claims);
        }

        // GET: /logout
        [Route("/[action]")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");
                await HttpContext.Authentication.SignOutAsync("oidc");
                return RedirectToAction("Logout");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
