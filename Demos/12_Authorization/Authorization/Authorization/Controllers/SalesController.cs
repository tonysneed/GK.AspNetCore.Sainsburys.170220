using Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authorization.Controllers
{
    //[Authorize(Roles = "Admin, Sales")]
    [Authorize("SalesPolicy")]
    public class SalesController : Controller
    {
        // GET: /<controller>/UK
        [Route("[controller]/{region}")]
        public IActionResult Index(string region)
        {
            var vm = new SalesData
            {
                Region = region,
                SalesTotal = region.ToUpper() == "US" ? 20000 : 10000
            };
            return View(vm);
        }
    }
}