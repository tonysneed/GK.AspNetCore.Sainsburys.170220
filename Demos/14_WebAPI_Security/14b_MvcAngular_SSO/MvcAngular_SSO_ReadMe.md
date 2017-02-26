## MvcAngular_SSO ReadMe

### Single Sign On with Identity Server in an MVC Core app with an Angular client

The **MvcAngularApp** project was created by create a new project and selecing the 
template for *ASP.NET Core Angular 2 Starter Application* from the C# Web 
category. The template is installed with the [ASP.NET Core Template Pack](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ASPNETCoreTemplatePack). 
For information on the project template, see Steve Sanderson's [blog post](http://blog.stevensanderson.com/2016/10/04/angular2-template-for-visual-studio/).

*NOTE: You can safely ignore the warning in the solution explorer for the MvcAngularApp 
stating that dependencies are not installed.*

The **QuickstartIdentityServer** project is from the [IdentityServer4.Samples](https://github.com/IdentityServer/IdentityServer4.Samples) 
GitHub repository. Instructions are based on steps outlined in the topic 
[Adding User Authentication with OpenID Connect](http://docs.identityserver.io/en/release/quickstarts/3_interactive_login.html) 
in the [IdentityServer4](http://identityserver.io/) [documentation](http://docs.identityserver.io/en/release/index.html). 

*NOTE: You may find it useful to set startup project for solution to current selection.*

1. Restore .NET and NPM packages

    - Restore .NET packages for each project

    ```
    dotnet restore
    ```

    - Install NPM packages for MvcAngularApp

    ```
    npm install
    ```

2. To add support for OpenID Connect authentication to **MvcAngularApp**, 
   add the following packages to project.json

    ```json
    "Microsoft.AspNetCore.Authentication.Cookies": "1.1.0",
    "Microsoft.AspNetCore.Authentication.OpenIdConnect": "1.1.0"
    ```

3. Add cookie and open id connect middleware to the app pipeline
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

4. Secure the web app using cookies authentication
    - Place an `[Authorize]` attribute on HomeController

5. Start both Identity Server and the MVC app
    - Select the QuickstartIdentityServer profile and press Ctrl+F5
      + You can use the IIS Express profile if you wish, but the 
        other profile will allow you to see messages in the console.
    - Press Ctrl+F5 to start the MVC app using IIS Express
      + This profile is required to support hot module replacement 
        for the Angular app
    - You should receive an error because the MVC app 
      is not yet registered with identity server

6. Configure Identity Server with scopes, clients and users
    - To configure Identity Server edit **Config.cs**
    - First add identity resources

    ```csharp
    // Scopes define the resources in your system
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        // Add identity resources
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    }
    ```

    - Next add OpenID Connect implicit flow client

    ```csharp
    // Clients want to access resources (aka scopes)
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            // Add OpenID Connect implicit flow client
            new Client
            {
                ClientId = "mvc",
                ClientName = "MVC Client",
                AllowedGrantTypes = GrantTypes.Implicit,

                RedirectUris = { "http://localhost:49934/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:49934" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };
    }
    ```

    - Then add users with credentials and claims

    ```csharp
    // Users require authentication and have claims
    public static List<TestUser> GetUsers()
    {
        // Add users with credentials and claims
        return new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password",

                Claims = new List<Claim>
                {
                    new Claim("name", "Alice"),
                    new Claim("website", "https://alice.com")
                }
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password",

                Claims = new List<Claim>
                {
                    new Claim("name", "Bob"),
                    new Claim("website", "https://bob.com")
                }
            }
        };
    }
    ```

    - Navigate to /secure again
      - This time you should be redirected to the Login page
      - Log in as alice or bob with the password: password
      - Accept the consent agreement
      - You should be redirected back to the home page

7. As a visual indication that the user has logged in, 
   you should display the user name on the navigation bar
    - Add AccountController to the Controllers folder and extend `Controller`
    - Add a Username action that returns the user's name if they are authenticated
    - Add an `[Authorize]` attribute to the Account controller

    ```csharp
     // GET: /username
    [HttpGet("/[action]")]
    public IActionResult Username()
    {
        string username = "";
        if (User.Identity.IsAuthenticated)
        {
            username = User.FindFirst("name").Value;
        }
        return Json(username);
    }
    ```

    - Edit navmenu.component.ts in ClientApp/app/components/navmenu 
      by adding a `loggedInAs` property
    - Set loggedInAs by using Http to call the username action
    - Add a `logoutUrl` property set to '/logout'
    - Add a `claimsUrl` property set to '/claims'

    ```js
    import { Component } from '@angular/core';
    import { Http } from '@angular/http';

    @Component({
        selector: 'nav-menu',
        template: require('./navmenu.component.html'),
        styles: [require('./navmenu.component.css')]
    })
    export class NavMenuComponent {

        // Call username action to display the logged in user's name
        public loggedInAs: string
        public claimsUrl: string = '/claims';
        public logoutUrl: string = '/logout';

        constructor(http: Http) {
            http.get('/username').subscribe(result => {
                let username = result.json();
                if(username.length > 0)
                this.loggedInAs = `Logged in as ${username}`;
            });
        }
    }
    ```

    - Update navmenu.component.html to display the user name 
      by adding a navbar-text bound to `loggedInAs`
      + Add it below the title

      ```html
      <p class='navbar-text'>{{loggedInAs}}</p>
      ```

      - Add menu items to show claims and to log out the user

    ```html
    <!-- Show user claims -->
    <li>
        <a href="{{claimsUrl}}">
            <span class='glyphicon glyphicon-user'></span> User claims
        </a>
    </li>
    <!-- Show a logout menu item -->
    <li>
        <a href="{{logoutUrl}}">
            <span class='glyphicon glyphicon-log-out'></span> Log out
        </a>
    </li>
    ```

8. Add a claims component to app/components

    First add a claims action to the Account controller

    ```csharp
    // GET: /claims
    [HttpGet("/[action]")]
    public IActionResult Claims()
    {
        var claims = from c in User.Claims select new { c.Type, c.Value };
        return Json(claims);
    }
    ```

    - Add a claims.component.ts file
    - Add a Claim interface
    - Call claims action to retreive an array of claims

    ```js
    import { Component } from '@angular/core';
    import { Http } from '@angular/http';

    @Component({
        selector: 'claims',
        template: require('./claims.component.html')
    })
    export class ClaimsComponent {

        public username: string
        public claims: Claim[];

        constructor(http: Http) {
            http.get('/username').subscribe(result => {
                this.username = result.json();
            });
            http.get('/claims').subscribe(result => {
                this.claims = result.json();
            });
        }
    }

    interface Claim {
        type: string;
        value: string;
    }
    ```

    - Add a claims.component.html file to show the claims

    ```html
    <h1>Claims for {{username}}</h1>

    <p>This component demonstrates retrieving user claims.</p>

    <p *ngIf="!claims"><em>Loading...</em></p>

    <table class='table' *ngIf="claims">
        <thead>
            <tr>
                <th>Type</th>
                <th>Value</th>
            </tr>
        </thead>
        <tbody>
            <tr *ngFor="let claim of claims">
                <td>{{ claim.type }}</td>
                <td>{{ claim.value }}</td>
            </tr>
        </tbody>
    </table>
    ```

    - Register claims component route in app.module.ts

    ```js
    import { NgModule } from '@angular/core';
    import { RouterModule } from '@angular/router';
    import { UniversalModule } from 'angular2-universal';
    import { AppComponent } from './components/app/app.component'
    import { NavMenuComponent } from './components/navmenu/navmenu.component';
    import { HomeComponent } from './components/home/home.component';
    import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
    import { CounterComponent } from './components/counter/counter.component';
    // Import claims component
    import { ClaimsComponent } from './components/claims/claims.component';

    @NgModule({
        bootstrap: [ AppComponent ],
        declarations: [
            AppComponent,
            NavMenuComponent,
            CounterComponent,
            FetchDataComponent,
            HomeComponent,
            // Declare claims component
            ClaimsComponent
        ],
        imports: [
            UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
            RouterModule.forRoot([
                { path: '', redirectTo: 'home', pathMatch: 'full' },
                { path: 'home', component: HomeComponent },
                { path: 'counter', component: CounterComponent },
                { path: 'fetch-data', component: FetchDataComponent },
                // Add claims route
                { path: 'claims', component: ClaimsComponent },
                { path: '**', redirectTo: 'home' }
            ])
        ]
    })
    export class AppModule {
    }
    ```

    - Select the User claims menu to view the user's claims

9. Add a Logout action to the Account controller

    ```csharp
    // GET: /logout
    [Route("/[action]")]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
            return RedirectToAction("Logout");
        }
        return RedirectToAction("Index", "Home");
    }
    ```

    - Select the Log out menu to log the user our of the app

*NOTE: The Web API actions have not yet been secured, so you will be able 
to navigate a browser to the urls and retrieve weather data. This will 
be recitfied in the next demo on Hybrid Flow.*
