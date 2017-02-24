// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4.Models;
using System.Collections.Generic;

namespace QuickstartIdentityServer
{
    using IdentityServer4;
    using System.Security.Claims;

    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("counter",new []{ "role", "admin", "user", "counter", "counter.admin", "counter.user" } ),
                new IdentityResource("fetchdata",new []{ "role", "admin", "user", "fetchdata", "fetchdata.admin", "fetchdata.user" } )
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("counter")
                {
                    ApiSecrets =
                    {
                        new Secret("counterscope".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "counter",
                            DisplayName = "Scope for the counter ApiResource"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "counter", "counter.admin", "counter.user" }
                },
                new ApiResource("fetchdata")
                {
                    ApiSecrets =
                    {
                        new Secret("fetchdata".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "fetchdatascope",
                            DisplayName = "Scope for the fetchdata ApiResource"
                        }
                    },
                    UserClaims = { "role", "admin", "user", "fetchdata", "fetchdata.admin", "fetchdata.user" }
                }
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials
            return new List<Client>
            {
                // TODO: Add the client app
                // new Client
                // {
                //     ClientName = "MVC Angular App",
                //     ClientId = "mvc",
                //     AllowedGrantTypes = GrantTypes.Implicit,
                //     AllowAccessTokensViaBrowser = true,
                //     RedirectUris = new List<string>
                //     {
                //         "http://localhost:49934/signin-oidc"
                //     },
                //     PostLogoutRedirectUris = new List<string>
                //     {
                //         "http://localhost:49934"
                //     },
                //     AllowedCorsOrigins = new List<string>
                //     {
                //         "http://localhost:49934"
                //     },
                //     AllowedScopes = new List<string>
                //     {
                //         IdentityServerConstants.StandardScopes.OpenId,
                //         IdentityServerConstants.StandardScopes.Profile,
                //         IdentityServerConstants.StandardScopes.Email,
                //     }
                // },
            };
        }
    }
}