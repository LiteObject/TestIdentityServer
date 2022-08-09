namespace Demo.Client
{
    using IdentityModel.Client;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var identityServiceClient = new HttpClient();

            // To retrieve the discovery document from IdentityService:
            var disco = await identityServiceClient.GetDiscoveryDocumentAsync(
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
            var tokenResponse = await identityServiceClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "my-console-client",
                ClientSecret = "secret",
                Scope = "demoapi.weatherforecast.read"                
            });

            Console.WriteLine($"JWT: {tokenResponse.Json}");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            try
            {
                using var apiClient = new HttpClient();
                apiClient.SetBearerToken(tokenResponse.AccessToken);

                var response = await apiClient.GetAsync("https://localhost:6001/WeatherForecast");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Respone status code from API: {statusCode}", response.StatusCode);
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message, e);
            }           
        }
    }
}
