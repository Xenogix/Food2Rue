using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Repositories;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Controllers
{
    public class WeatherForecastController : CRUDController<WeatherForecast, int>
    {
        public WeatherForecastController(WeatherForecastRepository repository) : base(repository)
        {
        }
    }
}