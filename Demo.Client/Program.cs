using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Client
{
    internal class Program
    {
        public static async Task Main()
        {
            Console.Title = "Console Client";

            using HttpClient identityServiceClient = new();

            using DiscoveryDocumentRequest discoveryDocumentRequest = new()
            {
                Address = "https://localhost:5001",
                Policy =
                    {
                        ValidateIssuerName = false
                    },
            };

            // To retrieve the discovery document from IdentityService:
            DiscoveryDocumentResponse disco = await identityServiceClient.GetDiscoveryDocumentAsync(discoveryDocumentRequest);

            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            using ClientCredentialsTokenRequest clientCredentialsTokenRequest = new()
            {
                Address = disco.TokenEndpoint,
                ClientId = "my-console-client",
                ClientSecret = "secret",
                Scope = "demoapi.one.read"
            };

            // To retrieve JWT token from IdentityService:
            TokenResponse tokenResponse = await identityServiceClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            Console.WriteLine($"JWT: {tokenResponse.Json}");

            if (tokenResponse.IsError)
            {
                Console.Error.WriteLine(tokenResponse.Error);
                return;
            }

            try
            {
                using HttpClient apiClient = new();
                apiClient.SetBearerToken(tokenResponse.AccessToken);

                Uri requestUri = new("https://localhost:6001/WeatherForecast");
                HttpResponseMessage response = await apiClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Respone status code from API: {response.StatusCode}");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message, ex);
            }
        }
    }
}
