using FDRWebsite.Server.Abstractions.Repository;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Repository
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        public async Task<bool> DeleteAsync(long key)
        {
            return true;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return new WeatherForecast[] { await GetAsync(0), await GetAsync(1), await GetAsync(2), await GetAsync(3) };
        }

        public async Task<WeatherForecast?> GetAsync(long key)
        {
            return new WeatherForecast()
            {
                Date = new DateOnly(2023, Random.Shared.Next(1, 12), Random.Shared.Next(1, 28)),
                TemperatureC = Random.Shared.Next(-5, 12),
                Summary = "This is my summary",
            };
        }

        public async Task<long> InsertAsync(WeatherForecast modelType)
        {
            return 0;
        }

        public async Task<bool> UpdateAsync(long key, WeatherForecast modelType)
        {
            return true;
        }
    }
}
