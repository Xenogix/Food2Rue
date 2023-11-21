using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Clients
{
    public interface IWeatherForecastClient : ICRUDApiClient<WeatherForecast , int>
    {
    }
}
