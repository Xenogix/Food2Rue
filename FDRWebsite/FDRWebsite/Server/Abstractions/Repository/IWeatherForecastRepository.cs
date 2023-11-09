using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Abstractions.Repository
{
    public interface IWeatherForecastRepository : IRepositoryBase<WeatherForecast, long>
    {
    }
}
