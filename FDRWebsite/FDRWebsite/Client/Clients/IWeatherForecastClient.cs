using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Clients
{
    public interface IWeatherForecastClient : IApiClientBase<WeatherForecast , int>
    {
    }
}
