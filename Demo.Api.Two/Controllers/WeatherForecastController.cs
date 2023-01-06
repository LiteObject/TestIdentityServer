using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Two.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private static readonly string[] scopeRequiredByApi = new string[] { "demoapi.one.read" };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize("read-policy")]
        public IEnumerable<WeatherForecast> Get()
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

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}