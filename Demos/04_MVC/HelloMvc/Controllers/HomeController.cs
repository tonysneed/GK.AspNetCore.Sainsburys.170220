using HelloMvc.Models;
using HelloMvc.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HelloMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomersRepository _customersRepo;

        public HomeController(ICustomersRepository customersRepo)
        {
            _customersRepo = customersRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _customersRepo.GetCustomer();
            return View(model);
        }

        public IActionResult Json()
        {
            var model = _customersRepo.GetCustomer();
            return Json(model);
        }

        [HttpPost("")]
        public IActionResult Edit([FromBody]Customer customer)
        {
            return View(customer);
        }
    }
}
