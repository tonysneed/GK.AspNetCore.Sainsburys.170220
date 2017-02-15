# HelloMVC ReadMe

1. Enable MVC
    - Add Microsoft.AspNetCore.Mvc to project.json
    - Startup.ConfigureServices: services.AddMvc()
    - Startup.Configure: app.UseMvc()

2. Add Controllers folder.
    - Right-click Controllers, Add New Item,  
      Select MVC Controller Class.
    - Rename Index to SayHello
    - Return Content as text/plain

3. Set routing
    - Try navigating to: http://localhost:5000/home/sayhello
        + Replace port number if using IIS Express
    - You should receive a 404 Not Found response
    - After app.UseMvc, call UseMvcWithDefaultRoute
    - Restart the app and refresh the page
    - You should now see Hello World displayed

4. Add an Index method
    - Return Content("Index")
    - Navigate to action
        + Go to http://localhost:5000/home/index
        + Go to http://localhost:5000/home
        + Go to http://localhost:5000
        + All three should return content

5. Add a Route attribute to SayHello
    - [Route("/hello-world")]
        + Go to http://localhost:5000/hello-world

6. Add Views/Home folders
    - Add MVC View called Index.cshtml
    
    ```html
    <!DOCTYPE html>
    <html>
    <head>
        <title>Index View</title>
    </head>
    <body>
        <h1>Hello from index.cshtml</h1>
    </body>
    </html>
    ```

    - Refactor Index method in Home controller 
      to return View.

7. Add markup with embedded C# code 
   and C# with embedded html

    ```html
    <div>The time is @DateTime.Now</div>
    @if (DateTime.Now.Hour < 12)
    {
        <div>Good morning</div>
    }
    else
    {
        <div>Good afternoon</div>
    }
    ```

8. Pass data to view with ViewData and ViewBag
    - Update Index in HomeController

    ```csharp
    ViewData["firstname"] = "John";
    ViewBag.lastname = "Doe";
    ```

    - Update Index.cshtml

    ```csharp
    @{ var firstName = ViewData["firstname"]; }
    ```

9. Create a CustomerViewModel class in a Models folder

    ```csharp
    public class CustomerViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    ```

    - Pass to View method in Index of HomeController

