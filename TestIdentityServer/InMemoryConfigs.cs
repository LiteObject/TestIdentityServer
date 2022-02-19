namespace TestIdentityServer
{
    using IdentityServer4;
    using IdentityServer4.Models;
    using IdentityServer4.Test;
    using System.Collections.Generic;
    using System.Security.Claims;

    /// <summary>
    /// The in memory configurations.
    /// </summary>
    public class InMemoryConfigs
    {
        /// <summary>
        /// The identity resources.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
                       {
                           new IdentityResources.OpenId(), new IdentityResources.Profile(),
                           new IdentityResources.Phone(), new IdentityResources.Email(), new IdentityResources.Address()
                       };
        }

        /// <summary>
        /// The API resources.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
                       {
                           new ApiResource("TestApp", "My Test Application")
                               {
                                   UserClaims = new[]
                                                    {
                                                        ClaimTypes.Email, ClaimTypes.HomePhone, ClaimTypes.MobilePhone,
                                                        ClaimTypes.OtherPhone, ClaimTypes.Role, "Profile", "CanCreateProfile",
                                                        "CanViewProfile"
                                                    }
                               }
                       };
        }

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            { new ApiScope("api1", "My API") };

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
                            ClientId = "client",
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            ClientSecrets = {
                                new Secret("secret".Sha256())
                            },
                            AllowedScopes = {
                                   IdentityServerConstants.StandardScopes.OpenId,
                                   IdentityServerConstants.StandardScopes.Profile,
                                   IdentityServerConstants.StandardScopes.Email,
                                   "api1" }
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
