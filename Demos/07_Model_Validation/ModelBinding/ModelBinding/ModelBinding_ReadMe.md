# ModelBinding ReadMe

1. Rename HomeViewModel to Person

    - Add an int Id property
    - Add an int Age property

2. Implement an IPersonRespository

    ```csharp
    public interface IPersonRepository
    {
        IEnumerable<Person> GetPersons();
        Person GetPerson(string name);
        void UpdatePerson(int id, Person person);
    }
    ```

    ```csharp
    public class PersonRepository : IPersonRepository
    {
        private readonly IDictionary<int, Person> _persons = new Dictionary<int, Person>
        {
            { 1, new Person { Id = 1, Name = "Peter", Age = 20 } },
            { 2, new Person { Id = 2, Name = "Paul", Age = 21 } },
            { 3, new Person { Id = 3, Name = "Mary", Age = 22 } },
        };

        public IEnumerable<Person> GetPersons()
        {
            return _persons.Values;
        }

        public Person GetPerson(int id)
        {
            return _persons[id];
        }

        public void UpdatePerson(int id, Person person)
        {
            _persons[id] = person;
        }
    }
    ```

    - Plug it into the DI system: Startup.ConfigureServices

    ```chsarp
    services.AddSingleton<IPersonRepository, PersonRepository>();
    ```

3. Update HomeController to inject IPersonRepository

    - Update Index.cshtml to return new with result from GetPersons.

    ```csharp
    public IActionResult Index()
    {
        var model = _personsRepo.GetPersons();
        return View(model);
    }
    ```

4. Update project.json to support tag helpers

    - Add MVC tag helpers to dependencies
    - Add razor tools to both dependencies and tools
    - Run `dotnet restore`

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

5. Add _ViewImports.cshtml to the  Views folder

    ```csharp
    @using ModelBinding
    @using ModelBinding.Models
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    ```

6. Update the Home View to display a list of persons

    ```html
    @model IEnumerable<Person>

    <!DOCTYPE html>
    <html>
    <head>
        <title>Persons List</title>
    </head>
    <body>
        <h2>Hello Model Binding!</h2>
        <hr/>
        <table>
            @foreach (var person in Model)
            {
                <tr>
                    <td>Id @person.Id: </td>
                    <td>@person.Name</td>
                    <td>Age @person.Age</td>
                    <td>
                        <a asp-action="Edit" asp-route-id="@person.Id">Edit</a>
                    </td>
                </tr>
            }
        </table>
    </body>
    </html>
    ```

7. Add an Edit GET action to the Home controller.

    ```csharp
    // GET: Home/Edit/John
    [HttpGet("Home/Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var model = _personsRepo.GetPerson(id);
        return View(model);
    }
    ```

8. Add an Edit view to the Home page.

    ```html
    @model Person

    <!DOCTYPE html>

    <html>
    <head>
        <meta name="viewport" content="width=device-width" />
        <title>Edit Person</title>
    </head>
    <body>

        <form asp-action="Edit">
            <h4>Edit Person</h4>
            <hr />
            <table>
                <tr>
                    <td><label asp-for="Name"></label></td>
                    <td><input asp-for="Name" /></td>
                </tr>
                <tr>
                    <td><label asp-for="Age"></label></td>
                    <td><input asp-for="Age" /></td>
                </tr>
            </table>
            <input type="submit" value="Save" />
        </form>

        <a asp-action="Index">Back to List</a>

    </body>
    </html>
    ```
9. Add an Edit POST action to the Home controller.

    - Should be able to edit persons

    ```csharp
    [HttpPost("Home/Edit/{id:int}")]
    public IActionResult Edit(int id, Person model)
    {
        _personsRepo.UpdatePerson(id, model);
        return RedirectToAction("Index");
    }
    ```

10. Add a Required attribute to Person.Name

    - Also add [Range(1, 100)] to Age

11. Update Edit in Home controller to check ModelState.IsValid

    ```csharp
    if (!ModelState.IsValid)
        return View(model);
    ```

12. Update Edit.cshtml with validation tag helpers

    - Within the form tag add asp-validation-summary

    ```html
    <div asp-validation-summary="All">
    ```

    - After each input add asp-validation-for

    ```html
    <td><span asp-validation-for="Name"></span></td>
    <td><span asp-validation-for="Age"></span></td>
    ```

13. Edit a person and try leaving the Name blank

    - Also set the age to more than 100
    - After clicking Save, you should see messages appear
    - Inspect traffic with Fiddler to see post backs

14. Enable client-side validation

    - Add scripts folder to wwwroot
      - jquery
      - jquery-validation
      - jquery-validation-unobtrusive

    - Add scripts to Edit.html

    ```html
    <script src="~/scripts/jquery/jquery.min.js"></script>
    <script src="~/scripts/jquery-validation/jquery.validate.min.js"></script>
    <script src="~/scripts/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
    ```

    - Verify with fiddler that no post backs occur on validation
15. 
