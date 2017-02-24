# OpenID Connect Hybrid Flow Authentication and API Access Tokens

This quickstart adds support for Google authentication.

1. Start identity server, web api and mvc client using IIS Express
    - QuickstartIdentityServer
    - Api
    - MvcClient

2. In the Mvc web client click the Secure link
    - Login as alice, password: password
    - Agree to the consent
    - Inspect the access_tokent claim
      - Try viewing it in jwt.io

3. Call Web API
   - Using user token
   - Using applicaion identity

4. Click Logout
5. 