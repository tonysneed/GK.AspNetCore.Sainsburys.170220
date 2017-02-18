using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepositoriesQueries.Models
{
    public class EditProductViewModel
    {
        public EditProductViewModel(Product product, 
            IEnumerable<Category> categories)
        {
            Product = product;
            Categories = new List<SelectListItem>();
            foreach (var category in categories)
            {
                Categories.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName
                });
            }
        }

        public Product Product { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }
}
