﻿using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fiver.Security.AuthServer
{
    //identity server on port 5000
    public static class Config
    {
        //api resource
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                //api on port 5001
                new ApiResource("fiver_auth_api", "Fiver.Security.AuthServer.Api")
            };
        }
        //get identity resource
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        //clients
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // Hybrid Flow = OpenId Connect + OAuth
                // To use both Identity and Access Tokens
                //client on port 5002
                new Client
                {
                    ClientId = "fiver_auth_client",
                    ClientName = "Fiver.Security.AuthServer.Client",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowOfflineAccess = true,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
                    
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "fiver_auth_api"
                    },
                },
                // Resource Owner Password Flow
                new Client
                {
                    ClientId = "fiver_auth_client_ro",
                    ClientName = "Fiver.Security.AuthServer.Client.RO",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowedScopes =
                    {
                        "fiver_auth_api"
                    },
                }
            };
        }

        //users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "james",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "James Bond"),
                        new Claim("website", "https://james.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "spectre",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Spectre"),
                        new Claim("website", "https://spectre.com")
                    }
                }
            };
        }
    }
}
