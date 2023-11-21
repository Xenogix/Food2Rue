using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Controllers
{
    public class WeatherForecastController : CRUDController<WeatherForecast, int>
    {
        public WeatherForecastController(IRepositoryBase<WeatherForecast, int> repository) : base(repository)
        {
        }
    }
}