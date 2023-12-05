using Refit;

namespace FDRWebsite.Client.Clients
{
    public static class RestClientRegister
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services, string baseUrl)
        {
            services.AddRefitClient(typeof(IWeatherForecastClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/weatherforecast"); });

            return services;
        }
    }
}
