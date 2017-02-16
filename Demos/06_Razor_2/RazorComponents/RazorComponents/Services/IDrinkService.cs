using System.Collections.Generic;
using System.Threading.Tasks;
using RazorComponents.Models;

namespace RazorComponents.Services
{
    public interface IDrinkService
    {
        Task<IEnumerable<Drink>> GetDrinks();
    }
}
