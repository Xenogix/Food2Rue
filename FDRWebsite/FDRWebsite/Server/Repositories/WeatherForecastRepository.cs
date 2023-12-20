using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Repositories
{
    public class WeatherForecastRepository : IRepositoryBase<WeatherForecast, int>
    {
        private const string TABLE_NAME = "weather";

        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            await Task.CompletedTask;
            return new WeatherForecast[] { await GetAsync(0), await GetAsync(1), await GetAsync(2), await GetAsync(3) };
        }

        public async Task<WeatherForecast?> GetAsync(int key)
        {
            await Task.CompletedTask;
            return new WeatherForecast()
            {
                Date = new DateOnly(2023, Random.Shared.Next(1, 12), Random.Shared.Next(1, 28)),
                TemperatureC = Random.Shared.Next(-5, 12),
                Summary = "This is my summary",
            };
        }

        public async Task<IEnumerable<WeatherForecast>> GetAsync(IFilter<WeatherForecast> modelFilter)
        {
            await Task.CompletedTask;
            return Array.Empty<WeatherForecast>();
        }

        public async Task<int> InsertAsync(WeatherForecast model)
        {
            await Task.CompletedTask;
            return model.ID;
        }

        public async Task<bool> UpdateAsync(int key, WeatherForecast model)
        {
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            await Task.CompletedTask;
            return true;
        }

    }
}
