using Microsoft.AspNetCore.Mvc;
using ModelBinding.Models;
using ModelBinding.Repositories;

namespace ModelBinding.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonRepository _personsRepo;

        public HomeController(IPersonRepository personsRepo)
        {
            _personsRepo = personsRepo;
        }


        // GET: Home/Index
        public IActionResult Index()
        {
            var model = _personsRepo.GetPersons();
            return View(model);
        }

        // GET: Home/Edit/John
        [HttpGet("Home/Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var model = _personsRepo.GetPerson(id);
            return View(model);
        }

        [HttpPost("Home/Edit/{id:int}")]
        public IActionResult Edit(int id, Person model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _personsRepo.UpdatePerson(id, model);
            return RedirectToAction("Index");
        }
    }
}
