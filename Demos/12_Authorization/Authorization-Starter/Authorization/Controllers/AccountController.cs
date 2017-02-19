using System.Linq;
using System.Security.Claims;
using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Authorization.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Providers = HttpContext.Authentication.GetAuthenticationSchemes()
                    .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
            };
            return View("Login", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Validate the user credentials
                // Match username and password - for demo purposes only
                if (model.Username != model.Password)
                {
                    ModelState.AddModelError("", "Invalid username or password");
                    return View("Login", model);
                }

                // Create subject and name claims
                var claims = new[]
                {
                    new Claim("sub", model.Username),
                    new Claim("name", model.Username),
                };

                // Create claims identity and principle
                var ci = new ClaimsIdentity(claims, "password", "name", "role");
                var cp = new ClaimsPrincipal(ci);

                // Sign in the user using the claims principle
                await HttpContext.Authentication.SignInAsync("Cookies", cp);

                // Redirect to the return url if present,
                // otherwise redirect to root
                if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return Redirect("~/");
            }

            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.Authentication.SignOutAsync("Cookies");
                return RedirectToAction("Logout");
            }
            return View("LoggedOut");
        }

        public IActionResult External(string provider, string returnUrl)
        {
            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }
            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            return new ChallengeResult(provider, props);
        }
    }
}
