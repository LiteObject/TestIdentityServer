using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Demo.Api.One.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public IdentityController(ILogger<WeatherForecastController> logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogDebug($"{nameof(Get)} has been invoked.");
            var claims = from c in User.Claims select new { c.Type, c.Value };

            return claims.Any() ? Ok(claims) : NotFound();
        }
    }
}
