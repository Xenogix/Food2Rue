using FDRWebsite.Server.Abstractions.Repository;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace FDRWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastRepository _forecastRepository;

        public WeatherForecastController(IWeatherForecastRepository forecastRepository)
        {
            _forecastRepository = forecastRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return await _forecastRepository.GetAsync();
        }
    }
}