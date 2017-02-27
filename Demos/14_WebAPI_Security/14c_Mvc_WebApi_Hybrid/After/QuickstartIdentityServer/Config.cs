// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace QuickstartIdentityServer
{
    public class Config
    {
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

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // Add a Web API resource
                new ApiResource("api1"),
            };
        }

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

        // Users require authentication and have claims
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                // Add users with credentials and claims
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
    }
}