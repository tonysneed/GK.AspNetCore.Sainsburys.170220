# RepositoriesQueries ReadMe

1. Add support for tag helpers and razor to project.json

    ```json
    // dependencies
    "Microsoft.AspNetCore.Mvc.TagHelpers": "1.1.1",
    "Microsoft.AspNetCore.Razor.Tools": {
      "version": "1.1.0-preview4-final",
      "type": "build"
    }

    // tools
    "Microsoft.AspNetCore.Razor.Tools": "1.1.0-preview4-final"
    ```

2. Add a _ViewImports.cshtml file to Views

    ```html
    @using RepositoriesQueries
    @using RepositoriesQueries.Models
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    ```

3. Add an ICategoryRepository.cs file to the Repositories folder.
    - Add a HelloWebApi.Repositories namespace
    - Add an ICategoryRepository interface

    ```csharp
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategory(int id);
    }
    ```

4. Add a CategoryRepository class that implements ICategoryRepository.
    - Add ctor that accepts ProductsDbContext
    - Use async / await for IO-bound async with 
      ToListAsync and SingleOrDefaultAsync

    ```csharp
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductsDbContext _context;

        public CategoryRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categories = await (from c in _context.Categories
                orderby c.CategoryName
                select c).ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories
                .SingleOrDefaultAsync(c => c.CategoryId == id);
            return category;
        }
    }
    ```

5. Add a CategoryController.cs file to the Controllers folder.
    - Right-click, Add New Item, MVC Controller
    - Add ctor to inject ICategoryRepository
    - Refactor Index to return `Task<IActionResult>`
    - Use async / await for IO-bound async

6. Generate an Index view for Category

    ```html
    @using RepositoriesQueries.Models
    @model IEnumerable<Category>

    <!DOCTYPE html>
    <html>
    <head>
        <title>Categories</title>
    </head>
    <body>
        <h2>Categories</h2>
        <hr />
        <table>
            @foreach (var category in Model)
            {
                <tr>
                    <td>Id @category.CategoryId: </td>
                    <td>@category.CategoryName</td>
                </tr>
            }
        </table>
    </body>
    </html>
    ```

7. Update Startup.ConfigureServices to add repository.

    ```csharp
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    ```

8. Run the app and browse to: http://localhost/category
    - You should see categories

9. Add an IProductRepository.cs file to the Repositories folder.

    ```csharp
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task LoadCategory(Product product);
    }
    ```

10. Add a ProductRepository class that implements IProductRepository.
    - Add ctor that accepts ProductsDbContext
    - Use async / await for IO-bound async
    - Include Category property in query for products

    ```csharp
    private readonly ProductsDbContext _context;

    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<Product> GetProduct(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.ProductId == id);
    }
    ```

    - For Create an Update set entity state then save changes.

    ```csharp
    public async Task<Product> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }
    ```

    - For the Delete method you need to retreive the product 
      by id in order to pass it to the repository Remove method.

    ```csharp
    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    ```

    - For the LoadCategory method you need to call LoadAsync 
      on the Reference of the product entry.

    ```csharp
    public async Task LoadCategory(Product product)
    {
        await _context.Entry(product)
            .Reference(p => p.Category)
            .LoadAsync();
    }
    ```

11. Add a Products view.

    - Display a list of products with an Edit link

    ```html
    @using RepositoriesQueries.Models
    @model IEnumerable<Product>

    <!DOCTYPE html>
    <html>
    <head>
        <title>Products</title>
    </head>
    <body>
        <h2>Products</h2>
        <hr />
        <table>
            @foreach (var product in Model)
            {
                <tr>
                    <td>@product.ProductId: </td>
                    <td>@product.ProductName </td>
                    <td>@product.UnitPrice</td>
                    <td>@product.Category.CategoryName</td>
                    <td>
                        <a asp-action="Edit" asp-route-id="@product.ProductId">Edit</a>
                    </td>
                </tr>
            }
        </table>
    </body>
    </html>
    ```

12. Update Startup.ConfigureServices to add product repository and unit of work.

    ```csharp
    services.AddScoped<IProductRepository, ProductRepository>();
    ```

    - Run the web app and browse to products.

13. Add an EditEditProductViewModel to Models.

    - Populate a SelectListItem collection for categories

    ```csharp
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
    ```

14. Add Edit action with [HttpGet] in Product controller to view a product.

    ```csharp
    // GET: /<controller>/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productRepo.GetProduct(id);
        var categories = await _categoryRepo.GetCategories();
        var vm = new EditProductViewModel(product, categories);
        return View(vm);
    }
    ```

15. Add a Product Edit view.

    ```html
    @model EditProductViewModel

    <!DOCTYPE html>
    <html>
    <head>
        <title>Edit Product</title>
    </head>
    <body>
        <form asp-controller="Product" asp-action="Edit" method="post">
            <h4>Edit Product</h4>
            <hr />
            <fieldset>
                <label asp-for="Product.ProductId"></label>
                <input asp-for="Product.ProductId" readonly="readonly" />
                <label asp-for="Product.ProductName"></label>
                <input asp-for="Product.ProductName" />
                <label asp-for="Product.UnitPrice"></label>
                <input asp-for="Product.UnitPrice" />
                <select type="text" asp-for="@Model.Product.CategoryId"
                        asp-items="@Model.Categories">
                    <option value="">--select--</option>
                </select>
            </fieldset>
            <input type="submit" value="Save" />
        </form>

        <a asp-action="Index">Back to Products</a>

    </body>
    </html>
    ```

16. Add Edit action with [HttpPost] in Product controller to view a product.

    - Use Bind attribute with "Product" prefix

    ```csharp
    // POST: /<controller>/Edit
    [HttpPost]
    public async Task<IActionResult> Edit([Bind(Prefix = "Product")]Product model)
    {
        await _productRepo.UpdateProduct(model);
        return RedirectToAction("Index");
    }
    ```

    - You should be able to edit and save products

