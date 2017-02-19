# Authentication ReadMe

1. Set up web app layout

    - Copy folders from _scripts to wwwroot/scripts
    - Add UseStaticFiles to Startup.Configure
    - Add Layout.cshtml to Views/Shared

    ```html
    <!DOCTYPE html>
    <html>
    <head>
        <meta name="viewport" content="width=device-width" />
        <link href="~/scripts/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
        <title>@ViewBag.title</title>
    </head>
    <body>
        <div class="container" id="main">
            @RenderBody()
        </div>
        <script src="~/scripts/jquery/jquery.min.js"></script>
        <script src="~/scripts/jquery-validation/jquery.validate.min.js"></script>
        <script src="~/scripts/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
        <script src="~/scripts/bootstrap/js/bootstrap.min.js"></script>
    </body>
    </html>
    ```

    - Add _ViewStart.cshtml to Views

    ```html
    @{
        Layout = "Layout";
    }
    ```

    - Refactor Home/Index.cshtml to use the layout
      + Leave @model and @inject directives
      + Remove everything but the contents of `<body>`
      + Set title to Home

    ```html
    @{
        ViewBag.Title = "Home";
    }
    ```

2. Enable tag helpers and razor tooling.

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

    - Add _ViewImports.cshtml to Views

    ```html
    @using Authentication
    @using Authentication.Models
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    ```

4. Enable cookie-based authentication.

    - Add cookies package to project.json

    ```json
    "Microsoft.AspNetCore.Authentication.Cookies": "1.0.1"
    ```

    - Add cookies auth to Startup.Configure before app.UseStaticFiles
    
    ```chsarp
    app.UseCookieAuthentication(new CookieAuthenticationOptions
    {
        AuthenticationScheme = "Cookies",
        AutomaticAuthenticate = true,
        AutomaticChallenge = true,
        ExpireTimeSpan = TimeSpan.FromMinutes(30),
        SlidingExpiration = true,
    });
    ```

4. Add controller and view for user login

    - Add LoginViewModel to Models

    ```csharp
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationDescription> Providers { get; set; }
    }
    ```

    - Add AccountController to Controllers
      + Add a Login action that returns a Login view, passing LoginViewModel
      + Add [AllowAnonymous] atribute so that non-authenticated users can view the page

    ```chsarp
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Login(string returnUrl)
        {
            var vm = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View("Login", vm);
        }
    }
    ```

    - Add Account/Login view

    ```html
    @model LoginViewModel
    @{
        ViewBag.Title = "Login";
    }
    <div class="page-header">
        <h2>@ViewBag.Title</h2>
    </div>
    <div class="row">
        <div class="col-sm-6">
            <div class="panel panel-default">
                <div class="panel-heading">Local Login</div>
                <div class="panel-body">
                    <form asp-action="Login">
                        <div asp-validation-summary="All"></div>

                        <input type="hidden" asp-for="@Model.ReturnUrl" />

                        <fieldset>
                            <div class="form-group">
                                <label asp-for="@Model.Username">Username</label>
                                <span asp-validation-for="@Model.Username" class="pull-right"></span>
                                <input type="text" asp-for="@Model.Username" class="form-control">
                            </div>

                            <div class="form-group">
                                <label asp-for="@Model.Password">Password</label>
                                <span asp-validation-for="@Model.Password" class="pull-right"></span>
                                <input type="password" asp-for="@Model.Password" class="form-control">
                            </div>

                            <div class="form-group">
                                <button class="btn btn-primary">Login</button>
                            </div>
                        </fieldset>
                    </form>
                </div>
            </div>
        </div>
    </div>
    ```

    - Run the app and go to /Account/Login
      + You should see the login view

5. Add an async Login action with an [HttpHost] action, 
   accepting a LoginViewModel

   - First validate user credentials
   - Create a new claims array with sub and name set to Username
   - Create claims identity and principle
   - Then sign in with the claims principle with Cookies auth
   - Redirect to return url

   ```csharp
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Validate the user credentials
            // Match username and password - for demo purposes only
            if (model.Username != model.Password)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View("Login", model);
            }

            // Create subject and name claims
            var claims = new[]
            {
                new Claim("sub", model.Username),
                new Claim("name", model.Username),
            };

            // Create claims identity and principle
            var ci = new ClaimsIdentity(claims, "password", "name", "role");
            var cp = new ClaimsPrincipal(ci);

            // Sign in the user using the claims principle
            await HttpContext.Authentication.SignInAsync("Cookies", cp);

            // Redirect to the return url if present,
            // otherwise redirect to root
            if (model.ReturnUrl != null && Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return Redirect("~/");
        }

        return View("Login", model);
    }
    ```

    - Now try logging in with a non-matching username and password
      + You should receive a validation error
    - Try logging in with a matching username and password
      + You should be redirected the the Home page

6. Now you need implement logout functionality
  - Add a Logout action to the Account controller
    + If the user is authenticated, sign the user out
      and redirect to the Logout action
    + If the user is not authenticated the return the LoggedOut view

    ```csharp
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            return RedirectToAction("Logout");
        }
        return View("LoggedOut");
    }
    ```
  - Add a LoggedOut view to the Account folder under Views

    ```html
    @{
        ViewBag.Title = "Logged Out";
    }
    <h2>You are now logged out.</h2>
    ```

  - Add a link on the Home view which executes the Logout action 
    on the Account controller
    + Run the app and ensure you can both login and logout

    ```html
    <a asp-controller="Account" asp-action="Logout">Logout</a>
    ```

7. Insert code in `Startup.ConfigureServices` to require authenticated users
  + Use a new auth policy builder to create a policy
    that requires users to be authenticated

    ```csharp
    // Require authenticated users
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    ```

  - In the AddMvc options, add a global auth filter

    ```csharp
    services.AddMvc(options =>
    {
        options.Filters.Add(new AuthorizeFilter(policy));
    });
    ```

    - Add a login link to the LoggedOut view redirecting to Home/Index

    ```html
    <a asp-controller="Home" asp-action="Index">Login</a>
    ```

    - Run the app and go to the home page
      + You should now be redirected to the login page
      + Login and you should be redirected to the home page

8. Show user name on Home page

    - In HomeController.Index, check if user is authenticated
    - If so, sent Name property of HomeViewModel to the user's name

    ```csharp
    public IActionResult Index()
    {
        string name = "Developer";
        if (User.Identity.IsAuthenticated)
        {
            name = User.Identity.Name;
        }
        var vm = new HomeViewModel { Name = name };
        return View(vm);
    }
    ```

    - Log in to see the user name shown on the home page

9. Add a global filter which defends against cross-site token forgery
    - Add a filter as an option in `services.AddMvc` in `Startup.ConfigureServices`

    ```charp
    options.Filters.Add(typeof(ValidateAntiForgeryTokenAttribute));
    ```

    - With your browser's F12 tools find the `<input name="__RequestVerificationToken">` 
      in the element explorer and delete it.
    - Then submit the page and you should get an error because the anti-xsrf token is invalid.

10. Add external authentication

    - Add to project.json:

    ```json
    "Microsoft.AspNetCore.Authentication.Google": "1.0.1"
    ```

    - Add the Google authentication middleware to Configure in ~/Startup.cs.
      + It needs to be after the cookie authentication middleware, but before the MVC middleware.

    ```csharp
    app.UseGoogleAuthentication(new GoogleOptions {
        AuthenticationScheme = "Google",
        SignInScheme = "Cookies",
        ClientId = "585966796531-oc9ab04c396lrj3lea09c51i7njb9gpj.apps.googleusercontent.com",
        ClientSecret = "oPmCaX1KsRUMl193Y9zwVnsP",
    });
    ```

    - Add code to Account controller that allows a user to trigger the external authentication

    ```csharp
    public IActionResult External(string provider, string returnUrl)
    {
        if (!Url.IsLocalUrl(returnUrl))
        {
            returnUrl = "/";
        }
        var props = new AuthenticationProperties { RedirectUri = returnUrl };
        return new ChallengeResult(provider, props);
    }
    ```

    - Update AccountController.Login to set Providers on LoginViewModel
    
    ```csharp
    public IActionResult Login(string returnUrl)
    {
        var vm = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            Providers = HttpContext.Authentication.GetAuthenticationSchemes()
                .Where(x => !string.IsNullOrWhiteSpace(x.DisplayName))
        };
        return View("Login", vm);
    }
    ```

    - Add external login links to Login.cshtml (before last div)

    ```html
    @if (Model.Providers != null && Model.Providers.Any())
    {
        <div class="col-sm-3 col-sm-offset-1">
            <div class="panel panel-default">
                <div class="panel-heading">External Login</div>
                <ul class="list-group">
                    @foreach (var provider in Model.Providers)
                    {
                        <li class="list-group-item">
                            <a asp-action="External"
                                asp-route-provider="@provider.AuthenticationScheme"
                                asp-route-returnurl="@Model.ReturnUrl"
                                class="btn btn-block btn-default">
                                <img src="~/images/@(provider.AuthenticationScheme).png">
                                @provider.DisplayName
                            </a>
                        </li>
                    }
                </ul>
            </div>
        </div>
    }
    ```

    - Log in using Google

