using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Api.One.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        private static readonly string[] scopeRequiredByApi = new string[] { "demoapi.one.read" };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize("read-policy")]
        // [RequiredScope(scopeRequiredByApi)]
        public async Task<IActionResult> Get()
        {
            // HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

            var claims = from c in User.Claims select new { c.Type, c.Value };

            foreach (var c in claims)
            {
                Console.WriteLine($"{c.Type}, {c.Value}");

                /*
                 * nbf - stands for not before.
                 * exp - stands for expiry.
                 * iss - stands for issuer.
                 * aud - stands for audience. The resource name in which a client is needed to access.
                 * client_id - the client id of the client application requesting the token.
                 * scope - the scope in which a client is allowed to access.
                 */
            }

            List<WeatherForecast> result = new();

            try
            {
                string bearerToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

                using HttpClient apiClient = new();
                apiClient.SetBearerToken(bearerToken);

                Uri requestUri = new("https://localhost:7001/WeatherForecast");
                HttpResponseMessage response = await apiClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Respone status code from API: {response.StatusCode}");
                    result = JsonConvert.DeserializeObject<List<WeatherForecast>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);

                    return StatusCode(StatusCodes.Status500InternalServerError, content);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message, ex);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return Ok(result);
        }

        [HttpPost]
        [Authorize("write-policy")]
        public async Task<IActionResult> Post(WeatherForecast payload)
        {
            await Task.Delay(2000);
            Console.WriteLine($"Created:\n{System.Text.Json.JsonSerializer.Serialize(payload)}");
            return Created("/", payload);
        }
    }
}
