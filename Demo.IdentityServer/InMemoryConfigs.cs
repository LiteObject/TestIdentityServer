using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Demo.IdentityServer
{
    /// <summary>
    /// The in memory configurations.
    /// https://localhost:5001/.well-known/openid-configuration
    /// </summary>
    public static class InMemoryConfigs
    {
        /*
         * In IdentityServer4 scopes are modelled as resources, which come in two flavors: IDENTITY and API. 
         * An IDENTITY RESOURCE allows you to model a scope that will return a certain set of claims, while 
         * an API RESOURCE scope allows you to model access to a protected resource/API.          
         */

        /// <summary>
        /// The identity resources.
        /// More: http://docs.identityserver.io/en/latest/reference/identity_resource.html
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
                       {
                           // some standard scopes from the OIDC spec
                           new IdentityResources.OpenId(),
                           new IdentityResources.Profile(),
                           new IdentityResources.Phone(),
                           new IdentityResources.Email(),
                           new IdentityResources.Address(),

                           // custom identity resource with some associated claims
                           new IdentityResource("custom.profile",
                           userClaims: new[] { JwtClaimTypes.Name, JwtClaimTypes.Email, "location", JwtClaimTypes.Address })
                       };
        }

        /// <summary>
        /// The API Resource Apis: the protected apis that Clients wants to access
        /// More: http://docs.identityserver.io/en/latest/reference/api_resource.html
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
                       {
                        /* new ApiResource("demo.test.api","Test Apis")
                         * 
                         * use this for simpler scenarios where you only require one scope per API. 
                         * In this case, the app.api.weather name will automatically become a scope 
                         * for the resource.
                         */
                           new ApiResource("demo.test.api","Test Apis"),
                           new ApiResource()
                            {
                                Name = "DemoApiOne",
                                DisplayName = "Demo-Api-One",
                                Description = "This is the Demo Api One to provide weather forecast",
                                ApiSecrets = { new Secret("api-secret".Sha256()) },
                                /*
                                *  List of associated user claims that should be included when this resource is requested.
                                */
                                UserClaims = new[]
                                                {
                                                    ClaimTypes.Email, ClaimTypes.HomePhone, ClaimTypes.MobilePhone,
                                                    ClaimTypes.OtherPhone, ClaimTypes.Role, "Profile",
                                                },

                                /*                                    
                                * The scope constrains the endpoints to which a client has access, and whether a client 
                                * has read or write access to an endpoint. 
                                * 
                                * Scopes this API resource allows
                                */
                                Scopes = new[] { "demoapi.one.read", "demoapi.one.write" }
                            },
                           new ApiResource()
                           {
                                Name = "DemoApiTwo",
                                DisplayName = "Demo-Api-Two",
                                Description = "This is the Demo Api Two to provide backend weather information",
                                ApiSecrets = { new Secret("api-secret".Sha256()) },
                                /*
                                *  List of associated user claims that should be included when this resource is requested.
                                */
                                UserClaims = new[]
                                                {
                                                    ClaimTypes.Email, ClaimTypes.HomePhone, ClaimTypes.MobilePhone,
                                                    ClaimTypes.OtherPhone, ClaimTypes.Role, "Profile",
                                                },
                                 /*                                    
                                * The scope constrains the endpoints to which a client has access, and whether a client 
                                * has read or write access to an endpoint. 
                                * 
                                * Scopes this API resource allows
                                */
                                Scopes = new[] { "demoapi.two.read", "demoapi.two.write" },
                           },
                       };
        }

        /// <summary>
        /// Define API scopes. Each scopes should be **unique** and that is why 
        /// it's recommended to define your scopes based on each API unique name.
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("demoapi.one.read", "Read API One"),
                new ApiScope("demoapi.one.write", "Write API One"),
                new ApiScope("demoapi.two.read", "Read API Two"),
                new ApiScope("demoapi.two.write", "Write API Two"),
            };

        /// <summary>
        /// The clients: applications that wants to access the Resource Apis.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<Client> Clients()
        {
            return new[]
                       {
                           new Client
                               {
                                   ClientId = "TestApp",
                                   ClientSecrets = new[] { new Secret("secret".Sha256()) },
                                   AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                                   AllowedScopes = new[]
                                                       {
                                                           IdentityServerConstants.StandardScopes.OpenId,
                                                           IdentityServerConstants.StandardScopes.Profile,
                                                           IdentityServerConstants.StandardScopes.Email,
                                                           IdentityServerConstants.StandardScopes.Phone,
                                                           IdentityServerConstants.StandardScopes.Email,
                                                           IdentityServerConstants.StandardScopes.Address,
                                                       },
                                   AllowOfflineAccess = true,
                                   AllowAccessTokensViaBrowser = true,
                                   //RedirectUris = { "" },
                                   //PostLogoutRedirectUris = { "" }                    
                               },
                           new Client
                            {
                                ClientId = "TestApp_implicit",
                                ClientSecrets = new[] { new Secret("secret".Sha256()) },
                                AllowedGrantTypes = GrantTypes.Implicit,
                                AllowedScopes = new[]
                                                    {
                                                        IdentityServerConstants.StandardScopes.OpenId,
                                                        IdentityServerConstants.StandardScopes.Profile,
                                                        IdentityServerConstants.StandardScopes.Email,
                                                        IdentityServerConstants.StandardScopes.Phone,
                                                        IdentityServerConstants.StandardScopes.Email,
                                                        IdentityServerConstants.StandardScopes.Address
                                                    },
                                AllowAccessTokensViaBrowser = true,
                                RedirectUris = new[] { "http://localhost:8406/signin-oidc" },
                                PostLogoutRedirectUris = new[] { "http://localhost:8406/signout-callback-oidc" }
                            },
                           new Client {
                            ClientId = "my-console-client",
                            ClientName = "DemoApiOneTwoClient",
                            Description = "This client interacts with the Weather API (aka Demo Api One) and provides weather forecast",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = {
                                new Secret("secret".Sha256())
                            },
                            /* 
                             * Specifies the api scopes that the client is allowed to request. 
                             */                            
                            // AllowedScopes = { "DemoWeatherApi" }
                            AllowedScopes = {
                                   IdentityServerConstants.StandardScopes.Email,
                                   "demoapi.one.read",
                                   "demoapi.one.write",
                                   "demoapi.two.read",
                                   "demoapi.two.write"
                            }
                           }
                       };
        }

        /// <summary>
        /// The users.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<TestUser> Users()
        {
            return new[]
                       {
                           new TestUser
                               {
                                   SubjectId = "1",
                                   Username = "testadminuser",
                                   Password = "password",
                                   IsActive = true,
                                   Claims = new List<Claim>()
                                                {
                                                    new Claim(ClaimTypes.Role, "Admin"),
                                                    new Claim(ClaimTypes.MobilePhone, "214-000-0000"),
                                                    new Claim(ClaimTypes.Email, "test.admin@test.com"),
                                                    new Claim("Profile", "CanCreateProfile"),
                                                    new Claim("Profile", "CanViewProfile")
                                                }
                               },
                           new TestUser
                               {
                                   SubjectId = "2",
                                   Username = "testuser",
                                   Password = "password",
                                   IsActive = true,
                                   Claims = new List<Claim>()
                                                {
                                                    new Claim(ClaimTypes.Role, "NonAdmin"),
                                                    new Claim(ClaimTypes.MobilePhone, "469-000-0000"),
                                                    new Claim(ClaimTypes.Email, "test.user@test.com"),
                                                    new Claim("Profile", "CanViewProfile")
                                                }
                               },
                       };
        }
    }
}
