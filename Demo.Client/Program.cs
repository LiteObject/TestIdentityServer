using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using HttpClient identityServiceClient = new();

            // To retrieve the discovery document from IdentityService:
            DiscoveryDocumentResponse disco = await identityServiceClient.GetDiscoveryDocumentAsync(
                new DiscoveryDocumentRequest
                {
                    Address = "https://localhost:5001",
                    Policy =
                    {
                        ValidateIssuerName = false
                    },
                });

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // To retrieve JWT token from IdentityService:
            TokenResponse tokenResponse = await identityServiceClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                // Scope = "openid"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine($"JWT: {tokenResponse.Json}");

            using HttpClient apiClient = new();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            HttpResponseMessage response = await apiClient.GetAsync("https://localhost:6001/api/identity");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Respone status code from API: {statusCode}", response.StatusCode);
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
