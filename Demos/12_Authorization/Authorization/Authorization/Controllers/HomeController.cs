using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Authorization.Models;
using Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authService;

        public HomeController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            string name = "Developer";
            if (User.Identity.IsAuthenticated)
            {
                name = User.Identity.Name;
            }

            //bool canViewSales = await _authService.AuthorizeAsync(User, "SalesPolicy");
            string[] viewableRegions = await GetViewableRegions(User, "US", "UK");
            bool canViewSales = viewableRegions.Length > 0;

            var vm = new HomeViewModel
            {
                Name = name,
                Role = User.FindFirst("role")?.Value ?? "None",
                Region = User.FindFirst("region")?.Value ?? "None",
                SecurityClearance = GetClearance(User),
                CanViewSales = canViewSales,
                ViewableRegions = viewableRegions
            };

            return View(vm);
        }

        private SecurityClearance GetClearance(ClaimsPrincipal user)
        {
            SecurityClearance clearance;
            Enum.TryParse(user.FindFirst("clearance")?.Value, out clearance);
            return clearance;
        }

        private async Task<string[]> GetViewableRegions(ClaimsPrincipal user,
            params string[] regions)
        {
            var regionsList = new List<string>();
            foreach (var region in regions)
            {
                bool canView = await _authService.AuthorizeAsync(User,
                    new SalesData {Region = region}, SalesOperations.View);
                if (canView) regionsList.Add(region);
            }
            return regionsList.ToArray();
        }
    }
}
