using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Api.Controllers
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

        static readonly string[] scopeRequiredByApi = new string[] { "demoapi.weatherforecast.read" };

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
                
        [HttpGet]
        [Authorize(Policy = "demoapi.weatherforecast.read")]
        // [RequiredScope(scopeRequiredByApi)]
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

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Authorize(Policy = "demoapi.weatherforecast.write")]
        public async Task<IActionResult> Post(WeatherForecast payload) 
        {
            await Task.Delay(2000);
            Console.WriteLine($"Created:\n{System.Text.Json.JsonSerializer.Serialize(payload)}");
            return Created("/", payload);
        }

        /*
         * If an authorization policy contains multiple authorization requirements, all requirements must 
         * pass in order for the policy evaluation to succeed. In other words, multiple authorization 
         * requirements added to a single authorization policy are treated on an AND basis.
         */

        [HttpGet("ping")]
        // [Authorize]
        [Authorize(Policy = "DisabledAuthOnPort80")]
        public IActionResult Ping() 
        {
            return Ok("I am alive!!!");
        }
    }
}
