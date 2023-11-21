using FDRWebsite.Client.Abstractions.Clients;
using Refit;
using System.Reflection;

namespace FDRWebsite.Client.Clients
{
    public static class RestClientRegister
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services, string baseUrl)
        {

            var interfaces = new Type[] { typeof(ICRUDApiClient<,>), typeof(IReadonlyApiClient<,>) };
            var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            var implementations = assemblyTypes.Where(
                x => !x.IsInterface &&
                x.GetInterfaces().Any(i => i.IsGenericType && interfaces.Any(type => type.IsAssignableFrom(i.GetGenericTypeDefinition()))));

            foreach (var genericType in implementations)
                services.AddRefitClient(genericType).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/weatherforecast"); });

            return services;
        }
    }
}
