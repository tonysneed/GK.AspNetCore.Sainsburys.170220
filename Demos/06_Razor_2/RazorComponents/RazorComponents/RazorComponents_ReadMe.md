# RazorComponents ReadMe

1. Add support for Razor to "tools" section of project.json.

    ```
    "Microsoft.AspNetCore.Razor.Tools": "1.0.0-preview2-final"
    ```

    - Run `dotnet restore`

2. Add MVC tag helpers to project.json

    ```
    "Microsoft.AspNetCore.Mvc.TagHelpers": "1.1.1"
    ```

3. Add _ViewImports.cshtml to the Views folder

    - Place common directives here.

    ```
    @using RazorComponents
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    ```

4. Copy contents of Index.cshtml to Views/Shared/_Layout.cshtml

    ```html
    @inject Microsoft.AspNetCore.Hosting.IHostingEnvironment Host
    <!DOCTYPE html>
    <html>
    <head>
        <title>@ViewBag.Title</title>
    </head>
    <body>
    @RenderBody()
    <hr/>
    <br/>
        <footer>
            <p>Environment: @Host.EnvironmentName</p>
        </footer>
    </body>
    </html>
    ```

5. Update Index.cshtml to leave just the body

    - Set Layout, ViewBag.Title

    ```html
    @model RazorComponents.Models.HomeViewModel
    @{
        Layout = "_Layout";
        ViewBag.Title = "Hello from MVC";
    }
    <div>
    <h2>Hello @Model.Name from MVC!</h2>
    </div>
    ```

6. Factor the footer out from the layout into a partial view.

    - Add a footer.cstml file to Shared.
    - Move the footer from _Layout.cshtml to footer.cshtml.
    - Add @model Microsoft.AspNetCore.Hosting.IHostingEnvironment
    - Replace @Host.EnvironmentName with @Model.EnvironmentName

    ```
    @model Microsoft.AspNetCore.Hosting.IHostingEnvironment
    <footer>
        <hr />
        <p>Environment: @Model.EnvironmentName</p>
    </footer>
    ```

    - Update _Layout.cshtml to use the partial view, passing Host

    ```
    @Html.Partial("footer", Host)
    ```

7. Flesh out an IDrinkService with an async GetDrinks method

    - Implement the interface with a DrinkService
    - Plug into DI in Startup.ConfigureServices

8. Add a ViewComponents folder

    - Add a DrinksViewComponent.cs file to the folder.
    - Inject IDrinkService into the ctor
    - Add InvokeAsync method

    ```csharp
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
    ```

9. Under Shared, add Components/Drinks folders

    - Add Drinks.cshtml

    ```html
    @using RazorComponents.Models
    @model List<Drink>

    <ol>
        @foreach (var drink in Model)
        {
            <li>
                @drink.Name @drink.Price.ToString("C")
            </li>
        }
    </ol>
    ```

10. Insert view component in Index.cshtml

    ```csharp
    @await Component.InvokeAsync("Drinks", 4)
    ```

