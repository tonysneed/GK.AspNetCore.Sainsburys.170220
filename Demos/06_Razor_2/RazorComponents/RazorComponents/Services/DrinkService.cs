using System.Collections.Generic;
using System.Threading.Tasks;
using RazorComponents.Models;

namespace RazorComponents.Services
{
    public class DrinkService : IDrinkService
    {
        public async Task<IEnumerable<Drink>> GetDrinks()
        {
            var drinks = new List<Drink>
            {
                new Drink {Name = "Expresso", Price = 5M},
                new Drink {Name = "Latte", Price = 4M},
                new Drink {Name = "Capuccino", Price = 3M},
                new Drink {Name = "Americano", Price = 2M},
                new Drink {Name = "Machiatto", Price = 1M},
            };
            return await Task.FromResult(drinks);
        }
    }
}
