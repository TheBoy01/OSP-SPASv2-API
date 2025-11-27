using Microsoft.AspNetCore.Mvc;
using OSP.SPASv2.Repository.Utility;
using Serilog.Filters;

namespace OSP.SPASv2.Repository.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
        //private ILogger<WaController> logger;

        private readonly ILogger<WeatherForecastController> _logger;
        private string _respmsg ;
        private TblResponse _tblResponse;
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _respmsg = string.Empty;
            _tblResponse = new TblResponse();
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

        //[HttpGet("GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{

        //    _respmsg = "Usercode - " + "Fetching - " + Utilities.Getprojectname() + " - " + Utilities.GetmethodName() + " - " + Request.Path.Value + "";
        //    _logger.LogInformation(_respmsg);

        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
    }
}