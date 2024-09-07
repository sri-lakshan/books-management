using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
      
        private readonly WeatherForecast cursor = new WeatherForecast();
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetBooks")]
        public IActionResult Get()
        {
            var weatherForecast = new WeatherForecast();
            var books = weatherForecast.GetBooks();

            return Ok(books);

        }

        [HttpPost("borrow/{id}")]
        public IActionResult BorrowBook(int id)
        {
            if (cursor.isAvailable(id)) 
            {
                cursor.borrowBook(id);

                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false, message = "Book is already borrowed" });
            }
            
        }

       

    }
}
