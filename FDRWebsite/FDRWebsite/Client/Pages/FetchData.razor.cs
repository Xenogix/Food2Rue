using Microsoft.AspNetCore.Components;
using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Pages
{
    public partial class FetchData
    {
        private IEnumerable<WeatherForecast>? forecasts;
        [Inject]
        public required IWeatherForecastClient weatherForecastClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            forecasts = await weatherForecastClient.GetAsync();
        }
    }
}