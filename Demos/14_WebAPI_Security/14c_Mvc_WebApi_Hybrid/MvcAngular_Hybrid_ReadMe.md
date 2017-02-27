## MvcAngular_Hybrid ReadMe

### Secure Web API's with Identity Server in an MVC Core app with an Angular client

The **MvcAngularApp** project was created by create a new project and selecing the 
template for *ASP.NET Core Angular 2 Starter Application* from the C# Web 
category. The template is installed with the [ASP.NET Core Template Pack](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ASPNETCoreTemplatePack). 
For information on the project template, see Steve Sanderson's [blog post](http://blog.stevensanderson.com/2016/10/04/angular2-template-for-visual-studio/).

*NOTE: You can safely ignore the warning in the solution explorer for the MvcAngularApp 
stating that dependencies are not installed.*

The **QuickstartIdentityServer** project is from the [IdentityServer4.Samples](https://github.com/IdentityServer/IdentityServer4.Samples) 
GitHub repository. Instructions are based on steps outlined in the topic 
[Switching to Hybrid Flow and adding API Access back](http://docs.identityserver.io/en/release/quickstarts/5_hybrid_and_api_access.html) 
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

2. Run the MvcAngularApp in IIS Express by pressing Ctrl+F5
    - Log into the application as alice with the password: password
    - Click the Fetch Data link to invoke the Web API service
    - Then open a browser and call the Web API directly:
      <http://localhost:49934/api/sampledata/weatherforecasts>
      + Notice that the Web API is not secured
      + If you were to place an [Authorize] on the SalesData controller,
        SalesData controller and submit the request via Postman or Fiddler,
        you would simply be redirected to the Login page.
      + This not the kind of behavior desired for non-web clients, 
        which should instead receive a 401 Unauthorized response.

3. Open **Config.cs** in QuickstartIdentityServer to edit the settings

    - First add a Web API resource called "api1" to `GetApiResources`

    ```csharp
    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            // Add a Web API resource
            new ApiResource("api1"),
        };
    }
    ```

    - Edit the `GetClients` method to allow HybridAndClientCredentials
    - Add "api1" to `AllowedScopes`
    - Add a client secret with "secret" as the shared secred
    - Set `AllowOfflineAccess` to true for requesting refresh tokens
    
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
                // Add secret for authenticating clients
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                // Allow the HybridAndClientCredentials grant type
                //AllowedGrantTypes = GrantTypes.Implicit,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                RedirectUris = { "http://localhost:49934/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:49934" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    // Allow access to the "api1" resource
                    "api1",
                },
                // Supply refresh tokens for long-lived access
                AllowOfflineAccess = true
            }
        };
    }
    ```

4. Edit UseOpenIdConnect of Startup.cs in MvcAngularApp

    - Configure the ClientSecret to match the secret at IdentityServer
    - Add api1 and offline_access scopes
    - Set the ResponseType to code id_token 
      (which basically means “use hybrid flow”)
    - Get claims from user info endpoint

    ```csharp
    // Add open id connect authentication
    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    app.UseOpenIdConnectAuthentication(new OpenIdConnectOptions
    {
        AuthenticationScheme = "oidc",
        SignInScheme = "Cookies",

        Authority = "http://localhost:5000",
        RequireHttpsMetadata = false,

        ClientId = "mvc",
        // Add client secret matching the server
        ClientSecret = "secret",

        // Add api1 and offline_access scopes
        Scope = { "api1", "offline_access" },

        // Set the ResponseType to code id_token for hybrid flow
        ResponseType = "code id_token",

        // Get claims from user info endpoint
        GetClaimsFromUserInfoEndpoint = true,

        SaveTokens = true
    });
    ```

    - Launch Identity Server as a console app and the Mvc-Anguler app
      using IIS Express.
      + Just make sure there are no errors and you can log in and out

5. Secure the Web API's in SampleData and Account controllers

    - Add a package to project.json for supporting validation of bearer tokens

    ```json
    // Add identity server support for bearer tokens
    "IdentityServer4.AccessTokenValidation": "1.0.5"
    ```

    - Add code to Startup.cs for validating bearer tokens

    ```csharp
    // Enable validation of bearer tokens
    app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
    {
        Authority = "http://localhost:5000",
        RequireHttpsMetadata = false,
        ApiName = "api1"
    });
    ```

    - Add an [Authorize] attribute to both Account and SampleData controllers
      + Specify the "Bearer" authentication scheme
    
    ```csharp
    [Authorize(ActiveAuthenticationSchemes = "Bearer")]
    ```

    - Submit a GET request to <http://localhost:49934/api/sampledata/weatherforecasts>
      - Use Postman or Fiddler
      - You should receive a response of 401 Unauthorized

6. Add an `AccessToken` action to the Account controller

    - Retrieve access token from the cookie
    
    ```csharp
    // GET: /account/accesstoken
    [HttpGet("account/[action]")]
    public IActionResult AccessToken()
    {
        var token = HttpContext.Authentication.GetTokenAsync("access_token");
        return Json(token);
    }
    ```

7. Access the protected Web API using the access token

    - After logging into the app, retrieve the access token
      + Open a browser tab to: <http://localhost:49934/account/accesstoken>
      + Copy the contents within the quote marks
    - Using Postman or Fiddler submit a GET request to the web api
      + GET: <http://localhost:49934/api/sampledata/weatherforecasts>
      + Add Authorization Header with a value: Bearer ACCESS_TOKEN
      + Paste the contents of the access token in the header
      + You should receive the data returned by the Web API
      + Removing the Authorization header will again result in a 401 response

8. Update the fetch-data component to use the access token

    - First select the Fetch data menu
      + No data will be retrieved because of a 401 response
    - Then refactor fetchdata.component.ts to first retrieve the access token
      + Change the Http import statement to include Headers and RequestOptions
      + Within the callback, set the authorization header on the request

    ```js
    constructor(http: Http) {
        // Get access token
        http.get('/account/accesstoken').subscribe(result => {
            if (result.status === 200) {
                let token = result.json();
                // Set authorization header using access token
                let headers = new Headers();
                headers.append('authorization', `Bearer ${token}`);
                let options = new RequestOptions({ headers: headers });
                // Use access token to invoke web api
                http.get('/api/SampleData/WeatherForecasts', options).subscribe(result => {
                    this.forecasts = result.json();
                });
            }
        });
    }
    ```

    - Fetch data will now successfully invoke the secured Web API

