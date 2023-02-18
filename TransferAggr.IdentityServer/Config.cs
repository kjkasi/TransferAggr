﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace TransferAggr.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "MyAPI")
        };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                    // machine-to-machine client (from quickstart 1)
                    new Client
                    {
                        ClientId = "client",
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        // scopes that client has access to
                        AllowedScopes = { "api1" }
                    },
                    // interactive ASP.NET Core Web App
                    new Client
                    {
                        ClientId = "web",
                        ClientSecrets = { new Secret("secret".Sha256()) },

                        AllowedGrantTypes = GrantTypes.Code,

                        // where to redirect after login
                        RedirectUris = { "http://localhost:5000/signin-oidc" },

                        // where to redirect after logout
                        PostLogoutRedirectUris = { "http://localhost:5000/signout-callback-oidc" },

                        AllowedScopes = new List<string>
                        {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "verification"
                        }
                    }
            };

    }
}
