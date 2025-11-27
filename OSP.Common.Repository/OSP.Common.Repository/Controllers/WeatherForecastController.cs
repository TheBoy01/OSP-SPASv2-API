using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace OSP.Common.Repository.Controllers
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
        //private readonly GenericService<MyEntity> _service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        //, GenericService<MyEntity> service
        {
            _logger = logger;
            //_service = service;
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

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var entities = await _service.GetAllAsync();
        //    return Ok(entities);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var entity = await _service.GetByIdAsync(id);
        //    if (entity == null)
        //        return NotFound();

        //    return Ok(entity);
        //}

        //[HttpPost]
        //public IActionResult Create([FromBody] MyEntity entity)
        //{
        //    if (entity == null)
        //        return BadRequest();

        //    _service.Insert(entity);
        //    _service.SaveAsync().Wait(); // This can be improved using async/await

        //    return CreatedAtAction("GetById", new { id = entity.Id }, entity);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody] MyEntity entity)
        //{
        //    if (entity == null || entity.Id != id)
        //        return BadRequest();

        //    _service.Update(entity);
        //    _service.SaveAsync().Wait(); // This can be improved using async/await

        //    return NoContent();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    _service.Delete(id);
        //    _service.SaveAsync().Wait(); // This can be improved using async/await

        //    return NoContent();
        //}
    }
}