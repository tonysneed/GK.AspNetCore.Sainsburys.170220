# WebAPI_Security ReadMe

## Demonstrates steps to secure web apps and API's using Identity Server

### Identity Server
- Home: <http://identityserver.io>
- Documentation: <http://docs.identityserver.io/en/release/index.html>
- Samples: <https://github.com/IdentityServer/IdentityServer4.Samples>

*NOTE: SSL is not required to use these demos, however SSL should be 
required in production in order to secure transmission of credentials 
as well as access tokens, which should be stored locally in a secure manner.*

---

1. **WebApi_Password**
    - Demonstrates *Resource Owner Flow* where credentials are sent to 
      Identity Server in order to obtain an access token, which is then 
      sent to a Web API for access to services.
    - This is how non-HTML clients should use Web API services.

2. **MvcAngular_SSO**
    - Demonstrates *Implicit Flow* for web applications where users are 
      redirected to the Identity Server login page when they attempt to 
      view a secure page, then allowed access to secure web pages.
    - Uses cookie-based authentication with Single SignOn for multiple 
      web applications.
    - In this scenario, Web API's are also secured with cookies and will 
      not be available for non-web clients.
    - POST actions will also require validation of an anti-forgery token.
    
3. **MvcAngular_Hybrid**
    - Demonstrates *Hybrid Flow* for web applications where users are 
      redirected to the Identity Server login page when they attempt to 
      view a secure page, then allowed access to secure web pages.
    - Like the previous sample, it usses cookie-based authentication with 
      Single SignOn for multiple web applications.
    - In this scenario, Web API's are *not* secured with cookies so that  
      they will also be available for non-web clients.
    - Instead, Web API's are secured with bearer tokens which are supplied 
      both by web and non-web clients.
