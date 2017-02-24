## SingleSignOn ReadMe

### Single sign-on with Identity Server in an MVC Core app with an Angular client

*NOTE: You may find it useful to set startup project for solution to current selection.*

The MvcAngularApp project was created by create a new project and selecing the 
template for **ASP.NET Core Angular 2 Starter Application** from the C# Web 
category. The template is installed with the [ASP.NET Core Template Pack](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ASPNETCoreTemplatePack). 
For information on the project template, see Steve Sanderson's [blog post](http://blog.stevensanderson.com/2016/10/04/angular2-template-for-visual-studio/).

*NOTE: You can safely ignore the warning in the solution explorer for the MvcAngularApp 
stating that dependencies are not installed.*

The IdentityServerWithAspNetIdentitySqlite project is from [AspNet5IdentityServerAngularImplicitFlow](https://github.com/damienbod/AspNet5IdentityServerAngularImplicitFlow/tree/VS2015_project_json) 
by [Damian Bod](https://damienbod.com/) and his blog post on 
[OAuth2 Implicit Flow with Angular and Identity Server](https://damienbod.com/2015/11/08/oauth2-implicit-flow-with-angular-and-asp-net-5-identity-server/).

Instructions are based on steps outlined in the topic 
[Adding User Authentication with OpenID Connect](http://docs.identityserver.io/en/release/quickstarts/3_interactive_login.html) 
in the [IdentityServer4](http://identityserver.io/) [documentation](http://docs.identityserver.io/en/release/index.html).

1. Restore .NET packages for each project

    ```
    dotnet restore
    ```

2. Install NPM packages for MvcAngularApp

    ```
    npm install
    ```

3. Initialize IdentityServer database

    - Replace connection string in appSettings.json to local path for usersdatabase.sqlite
        + Select in Solution Explorer, press F4, copy path, paste into appSettings.json
    - Apply migrations

    ```
    dotnet ef database update
    ```

4. Start the identity server using Kestrel
    - Select the IdentityServerWithAspNetIdentitySqlite profile
    - Press Ctrl+F5 to start the app
    - A browser will open at: <http://localhost:5000/>
    - Click Login and register a new user
      + Email: alice@foo.com
      + Password: Pa$$w0rd
      + IsAdmin: Yes
    - After logging in, go ahead and log out

5. Start the MvcAngularApp app using IIS Express
    - Press Ctrl+F5 to start the app
    - A browser will open at: <http://localhost:49934/>
    - If it looks alright go ahead and stop the app

6. To add support for OpenID Connect authentication to the MVC application, 
   add the following packages to project.json

    ```json
    "Microsoft.AspNetCore.Authentication.Cookies": "1.1.0",
    "Microsoft.AspNetCore.Authentication.OpenIdConnect": "1.1.0"
    ```

7. Add cookie and open id connect middleware to the app pipeline
    - Add cookie auth in Startup.Configure
    - Place it before app.UseStaticFiles

    ```csharp
    app.UseCookieAuthentication(new CookieAuthenticationOptions
    {
        AuthenticationScheme = "Cookies"
    });
    ```

    - After this code add open id connect auth

    ```csharp
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

    app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
    {
        AuthenticationScheme = "oidc",
        SignInScheme = "Cookies",

        Authority = "http://localhost:5000",
        RequireHttpsMetadata = false,

        ClientId = "mvc",
        SaveTokens = true
    });
    ```

    - Add a Secure action to the Home controller
        - Place [Authorize] attribute on the method
        - Pass an array of user claims to the view

    ```chsarp
    [Authorize]
    [Route("secure")]
    public IActionResult Secure()
    {
        var claims = User.Claims.ToArray();
        return View(claims);
    }
    ```

    - Add a Secure.cshtml file to Views/Home

    ```html
    @using System.Security.Claims
    @model Claim[]

    @{
        ViewData["Title"] = "Secure";
    }

    <div class="container" id="main">
        <h2>User Claims</h2>
        <dl>
            @foreach (var claim in User.Claims)
            {
                <dt>@claim.Type</dt>
                <dd>@claim.Value</dd>
            }
        </dl>
    </div>
    ```

    - Press Ctrl+F5 to start the app
    - Browse to /secure
        - You should receive an error because the MVC app 
          is not yet registered with identity server

8. Add Mvc client to identity server

    - Open Config.cs in identity server

    ```csharp
    new Client
    {
        ClientName = "MVC Angular App",
        ClientId = "mvc",
        AllowedGrantTypes = GrantTypes.Implicit,
        AllowAccessTokensViaBrowser = true,
        RedirectUris = new List<string>
        {
            "http://localhost:49934/signin-oidc"
        },
        PostLogoutRedirectUris = new List<string>
        {
            "http://localhost:49934"
        },
        AllowedCorsOrigins = new List<string>
        {
            "http://localhost:49934"
        },
        AllowedScopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            IdentityServerConstants.StandardScopes.Email,
        }
    },
    ```

    - Navigate to /secure again
      - This time you should be redirected to the Login page
      - Log in as alice@foo.com with the password: Pa$$w0rd
      - Accept the persmission
      - You should be redirected back to secure to view claims

9. Add a Logout action to the Home controller

    ```csharp
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
            return RedirectToAction("Logout");
        }
        return View("Index");
    }
    ```

    - Add an `[Authorize]` attribute to the Index action in the Home controller
    - Navigate to /logout
      - You should be logged out and redirected to the Login page


