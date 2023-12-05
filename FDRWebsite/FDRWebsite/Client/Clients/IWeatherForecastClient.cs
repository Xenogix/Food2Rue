using FDRWebsite.Shared.Models;
using Refit;

namespace FDRWebsite.Client.Clients
{
    public interface IWeatherForecastClient : ICRUDApiClient<WeatherForecast , int>
    {
    }
}
