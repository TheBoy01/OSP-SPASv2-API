using _1SP.SPASv2.Service.Model;
using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Service;
using Microsoft.AspNetCore.Authorization;
using OSP.SPASv2.Service.Model;

namespace _1SP.SPASv2.Service.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Daboy", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        //private readonly ILogger<WeatherForecastController> _logger;

        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly JWTAuthenticationManager jwtAuthenticationManager;


        public WeatherForecastController(JWTAuthenticationManager jwtAuthenticationManager)
        {
            this.jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [AllowAnonymous]
        [HttpPost("Authorize")]
        public IActionResult AuthUser([FromBody] User user)
        {
            var token = jwtAuthenticationManager.Authenticate(user.Username, user.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}