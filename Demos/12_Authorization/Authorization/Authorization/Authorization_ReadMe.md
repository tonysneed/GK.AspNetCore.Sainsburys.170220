# Authorization ReadMe

*NOTE: The Starter project is set up to require authentication.*

1. Add Sales controller with view model and view

    - Add a SalesData class to Models

    ```csharp
    public class SalesData
    {
        public string Region { get; set; }
        public decimal SalesTotal { get; set; }
    }
    ```

    - Add SalesController class to Controllers

    ```csharp
    public class SalesController : Controller
    {
        // GET: /<controller>/UK
        [Route("[controller]/{region}")]
        public IActionResult Index(string region)
        {
            var vm = new SalesData
            {
                Region = region,
                SalesTotal = region.ToUpper() == "US" ? 20000 : 10000
            };
            return View(vm);
        }
    }
    ```

    - Add Index.cshtml to Views/Sales

    ```html
    @model SalesData
    @{
        ViewBag.Title = "Sales";
    }

    <h2>Sales Report</h2>
    <h4>Region: <strong>@Model.Region</strong></h4>
    <h4>Total Sales: @Model.SalesTotal.ToString("C")</h4>
    <p><a asp-controller="Home" asp-action="Index">Home</a></p>
    ```

2. Update home controller, view model and view to include role and region

    - HomeViewModel

    ```csharp
    public enum SecurityClearance
    {
        None,
        Confidential,
        Secret,
        TopSecret,
        WikiLeaks
    }

    public class HomeViewModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Region { get; set; }
        public SecurityClearance SecurityClearance { get; set; }
    }
    ```

    - HomeController

    ```csharp
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            string name = "Developer";
            if (User.Identity.IsAuthenticated)
            {
                name = User.Identity.Name;
            }
            var vm = new HomeViewModel
            {
                Name = name,
                Role = "None",
                Region = "None",
                SecurityClearance = SecurityClearance.None
            };
            return View(vm);
        }
    }
    ```

    - Home/Index view

    ```html
    @model HomeViewModel
    @inject Microsoft.AspNetCore.Hosting.IHostingEnvironment Host
    @{
        ViewBag.Title = "Home";
    }

    <h2>Hello Authorization from MVC!</h2>
    <p>User Name: <strong>@Model.Name</strong></p>
    <p>Role: <strong>@Model.Role</strong></p>
    <p>Region: <strong>@Model.Region</strong></p>
    <p>Security Clearance: <strong>@Model.SecurityClearance</strong></p>
    <hr/>
    <p>
        View Sales:
        <a asp-controller="Sales" asp-action="Index" asp-route-region="US">US</a>
        <a asp-controller="Sales" asp-action="Index" asp-route-region="UK">UK</a>
    </p>
    <hr />
    <p><a asp-controller="Account" asp-action="Logout">Logout</a></p>
    Environment: <strong>@Host.EnvironmentName</strong>
    ```

    - Run the app to make sure it works

3. Apply an Authorize to the Sales controller for Admin and Sales roles

    ```
    [Authorize(Roles = "Admin, Sales")]
    ```

    - Run the app and go to sales
      + You should receive an Access Denied 404 response

4. Create a service for assigning claims to users

    - Add a Services folder
    - Create an IUserClaimsService interface

    ```csharp
    public interface IUserClaimsService
    {
        Claim[] GetUserClaims(string userName);
    }
    ```

    - Implement the interface with a UserClaimsService class

    ```csharp
    public Claim[] GetUserClaims(string userName)
    {
        var claimsList = new List<Claim>
        {
            new Claim("sub", userName),
            new Claim("name", userName),
        };

        switch (userName)
        {
            case "Tony":
                claimsList.Add(new Claim("role", "Admin"));
                break;
            case "Alice":
                claimsList.Add(new Claim("role", "Sales"));
                claimsList.Add(new Claim("region", "UK"));
                claimsList.Add(new Claim("clearance", "Secret"));
                break;
            case "Bob":
                claimsList.Add(new Claim("role", "Sales"));
                claimsList.Add(new Claim("region", "US"));
                claimsList.Add(new Claim("clearance", "TopSecret"));
                break;
            case "Julian":
                claimsList.Add(new Claim("clearance", "WikiLeaks"));
                break;
        }
        return claimsList.ToArray();
    }
    ```

    - Register the service with DI in `Startup.ConfigureServices`

    ```csharp
    services.AddScoped<IUserClaimsService, UserClaimService>();
    ```

5. Inject IUserClaimsService into Account controller

    - Replace `var claims = new[]` with:

    ```csharp
    var claims = _userClaimsService.GetUserClaims(model.Username);
    ```

6. Refactor code in the Home controller that sets the vm properties

    ```csharp
    var vm = new HomeViewModel
    {
        Name = name,
        Role = User.FindFirst("role")?.Value ?? "None",
        Region = User.FindFirst("region")?.Value ?? "None",
        SecurityClearance = GetClearance(User)
    };

    SecurityClearance GetClearance(ClaimsPrincipal user)
    {
        SecurityClearance clearance;
        Enum.TryParse(user.FindFirst("clearance")?.Value, out clearance);
        return clearance;
    }
    ```

    - Run the app and log in as different users: Tony, Alice, Bob, Julian
    - Try viewing the sales page
      + Only Admin and Sales roles may see the page

7. At this point we want to allow users with a WikiLeaks clearance 
   to see the Sales page. But this is where roles-based security breaks down.

    - Add an authorization policy 

    - Add a sales policy that requires the user be in the Sales role

    - Call `services.AddAuthorization` in `Startup.ConfigureServies` 
      to add a policy called "SalesPolicy".
        + Use RequireAssertion to allow Admin role, Sales role, or 
          a clearance of WikiLeaks

    ```csharp
    services.AddAuthorization(options =>
    {
        options.AddPolicy("SalesPolicy", builder =>
        {
            builder.RequireAssertion(context => 
                context.User.HasClaim("role", "Admin") ||
                context.User.HasClaim("role", "Sales") ||
                context.User.HasClaim("clearance", "WikiLeaks"));
        });
    });
    ```

    - Change `[Authorize]` on the Sales controller to specify the policy

    ```csharp
    ```

    - Users with WikiLeaks clearance can now view Sales

8. Rather than returning a 404, we can perform the auth check from the 
   programmatically to show or hide the Sales link on the home view.

   - Add a `bool` `CanViewSales` property to HomeViewModel
   - Inject `IAuthorizationService` into the home controller
   - Use async / await to call AuthorizeAsync
   - Set `CanViewSales` to result of AuthorizeAsync

    ```csharp
    bool canViewSales = await _authService.AuthorizeAsync(User, "SalesPolicy");
    ```

    - Check CanViewSales in the home view

    ```html
    @if (Model.CanViewSales)
    {
        <div>
            <hr />
            View Sales:
            <a asp-controller="Sales" asp-action="Index" asp-route-region="US">US</a>
            <a asp-controller="Sales" asp-action="Index" asp-route-region="UK">UK</a>
        </div>
    }
    ```

9. Lastly, we'll use resource-based auth to allow users to see only 
   the sales link for their own region.

    - Create a Policies folder
    - Add static SalesOperations class

    ```csharp
    public static class SalesOperations
    {
        private const string _view = "View";

        public static readonly OperationAuthorizationRequirement View =
            new OperationAuthorizationRequirement { Name = _view };
    }
    ```

    - Add SalesViewPolicy class
      + Extend AuthorizationHandler
      + Specify type args: OperationAuthorizationRequirement, SalesData
      + Override Handle method

    ```csharp
    public class SalesViewPolicy : AuthorizationHandler
        <OperationAuthorizationRequirement, SalesData>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, SalesData resource)
        {
            if (requirement == SalesOperations.View)
            {
                if (context.User.HasClaim("role", "Admin") ||
                    context.User.HasClaim("clearance", "WikiLeaks") ||
                    context.User.HasClaim("role", "Sales") &&
                    context.User.HasClaim("region", resource.Region))
                {
                    context.Succeed(requirement);
                }
            }
            return Task.FromResult(0);
        }
    }
    ```

    - Register the handler in Startup.ConfigureServices

    ```csharp
    services.AddScoped<IAuthorizationHandler, SalesViewPolicy>();
    ```

    - Refactor home view model and controller with viewable regions
        + Add property to HomeViewModel

    ```csharp
    public string[] ViewableRegions { get; set; }
    ```

    - Add GetViewableRegions method to home controller

    ```csharp
    private async Task<string[]> GetViewableRegions(ClaimsPrincipal user,
        params string[] regions)
    {
        var regionsList = new List<string>();
        foreach (var region in regions)
        {
            bool canView = await _authService.AuthorizeAsync(User,
                new SalesData {Region = region}, SalesOperations.View);
            if (canView) regionsList.Add(region);
        }
        return regionsList.ToArray();
    }
    ```

    - Remove or coment out declaration of `canViewSales`
      + Replace with call to `GetViewableRegions`

    ```csharp
    bool canViewSales = viewableRegions.Length > 0;
    ```

    - Run the app to see results
      - Tony and Julian can see both US and UK sales
      - Alice can see UK only
      - Bob can see US only
      - Others cannot see any

