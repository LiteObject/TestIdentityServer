﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Random rng = new();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
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
