using FDRWebsite.Server.Abstractions.Repository;

namespace FDRWebsite.Server.Repository
{
    public static class RepositoryRegister
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
        }
    }
}
