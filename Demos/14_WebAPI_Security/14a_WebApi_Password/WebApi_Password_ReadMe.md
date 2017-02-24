## IdentityServer_Quickstart ReadMe

### Demonstrates use of Identity Server to secure a Web API

*NOTE: You may find it useful to set startup project for solution to current selection.*

1. Test the Api and Client without authentication

    - Start the Api project by pressing Ctrl + F5
        + Note that a browser will not launch
    - Start the ResourceOwnerClient project by pressing Ctrl + F5
        + Press Enter to invoke the web api
        + You should see a message and empty claims:

    ```
    Message:
    [
      {
        "message": "Hello from Web API"
      }
    ]

    Claims:
    []
    ```

2. Secure the Web API

    - In project.json uncomment the dependency for 
      **IdentityServer4.AccessTokenValidation**

    - In startup.cs add identity server auth middleware
        + In options set Authority to identity server's URL
        + Indicate that https is noe required
        + Set ApiName to "api1"

    ```
    app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
    {
        Authority = "http://localhost:5000",
        RequireHttpsMetadata = false,
                
        ApiName = "api1"
    });
    ```

    - Add `[Authorize]` attribute to ClaimsController
        + This will disallow anonymous requests

3. Test Api and client with authentication required by the Api

    - Restart the Api project
    - Press Enter to invoke the Web API
    - You should receive unauthorized responses

4. Start QuickstartIdentityServer by pressing Ctrl + F5

    - Identity server will be able to issue access tokens

5. Update the client to request a token from identity server

    - Note the dependency on IdentityModel in project.json
    - In Program.Main prompt for username and password
    - In the GetToken method discover the token endpoint
    - Then request a token from identity server
        + Call RequestResourceOwnerPasswordAsync, passing username and password
        + Return the tokenResponse
        + Remove the call to Task.FromResult
    - In GetApiResponse call SetBearerToken on the HttpClient

6. Run ResourceOwnerClient by pressing Ctrl + F5

    - Press Y to authenticate the user
    - Press Enter twice to accept the default username and password
    - Press Enter to discover endpoints from metadata
    - Press Enter to retrieve access token from service
        + You should see an access token shown
    - Press Enter to invoke Web API
        + You should see both a message and set of claims
    - If you like, you can paste the contents of the access token 
      into the Encoded textbox at <http://jwt.io>
        + You should see the decoded token on the right
        + Note that the token is not encrypted or signed


