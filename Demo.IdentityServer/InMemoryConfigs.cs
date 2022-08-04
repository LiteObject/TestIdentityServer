namespace TestIdentityServer
{
    using IdentityServer4;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;
    using System.Security.Claims;

    /// <summary>
    /// The in memory configurations.
    /// https://localhost:5001/.well-known/openid-configuration
    /// </summary>
    public class InMemoryConfigs
    {
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
                           new IdentityResources.OpenId(), 
                           new IdentityResources.Profile(),
                           new IdentityResources.Phone(), 
                           new IdentityResources.Email(), 
                           new IdentityResources.Address()
                       };
        }

        /// <summary>
        /// The API resources.
        /// More: http://docs.identityserver.io/en/latest/reference/api_resource.html
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
                       {
                            // The API we sih to protect
                           new ApiResource()
                               {
                                   Name = "DemoWeatherApi",
                                   DisplayName = "DemoApi-Weather-Service",
                                   Description = "This is a demo api to provide weather forecast",
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
                                   Scopes = new[] { "demoapi.weatherforecast.read", "demoapi.weatherforecast.write" }
                               }
                       };
        }

        /// <summary>
        /// Define API scopes
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { 
                new ApiScope("demoapi.weatherforecast.read", "Read Weather API"),
                new ApiScope("demoapi.weatherforecast.write", "Write Weather API")
            };        

    /// <summary>
    /// The clients.
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
                            ClientName = "WeatherForecastClient",
                            Description = "This client interacts with the Weather API and provides weather forecast",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = {
                                new Secret("secret".Sha256())
                            },
                            /* 
                             * Specifies the api scopes that the client is allowed to request. 
                             */
                            AllowedScopes = {
                                   /* IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Profile,
                                   IdentityServerConstants.StandardScopes.Email, */
                                   "demoapi.weatherforecast.read",
                                   "demoapi.weatherforecast.write"
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
