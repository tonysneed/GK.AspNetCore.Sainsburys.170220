using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace MvcAngularApp.Controllers
{
    [Authorize]
    public class AcountController : Controller
    {
        // GET: /account/username
        [HttpGet("account/[action]")]
        public IActionResult Username()
        {
            string username = "";
            if (User.Identity.IsAuthenticated)
            {
                username = User.FindFirst("name").Value;
            }
            return Json(username);
        }

        // GET: /account/claims
        [HttpGet("account/[action]")]
        public IActionResult Claims()
        {
            var claims = from c in User.Claims select new { c.Type, c.Value };
            return Json(claims);
        }

        // GET: /account/accesstoken
        [HttpGet("account/[action]")]
        public async Task<IActionResult> AccessToken()
        {
            // Retreive access token from the authorization cookie
            var token = await HttpContext.Authentication.GetTokenAsync("access_token");
            return Json(token);
        }

        // GET: /account/logout
        [Route("account/[action]")]
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
