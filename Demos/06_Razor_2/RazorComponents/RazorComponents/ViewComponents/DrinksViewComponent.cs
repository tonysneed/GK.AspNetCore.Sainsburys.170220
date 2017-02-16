using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RazorComponents.Services;

namespace RazorComponents.ViewComponents
{
    public class DrinksViewComponent : ViewComponent
    {
        private readonly IDrinkService _drinkService;

        public DrinksViewComponent(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }
        public async Task<IViewComponentResult> InvokeAsync(int count)
        {
            var drinks = await _drinkService.GetDrinks();
            var result = drinks.Take(count).ToList();
            var view = View("Drinks", result);
            return await Task.FromResult<IViewComponentResult>(view);
        }
    }
}
