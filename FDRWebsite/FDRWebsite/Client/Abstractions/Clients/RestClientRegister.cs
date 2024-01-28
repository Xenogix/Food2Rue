using FDRWebsite.Client.Clients;
using Refit;

namespace FDRWebsite.Client.Abstractions.Clients
{
    public static class RestClientRegister
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services, string baseUrl)
        {
            services.AddRefitClient(typeof(IAuthenticationClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/authentication"); });
            services.AddRefitClient(typeof(IPublicationClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/publication"); });
            services.AddRefitClient(typeof(IUtilisateurClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/utilisateurs"); });
            services.AddRefitClient(typeof(IPaysClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/pays"); });
            services.AddRefitClient(typeof(IImageClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/image"); });
            services.AddRefitClient(typeof(IFileClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/files"); });

            return services;
        }
    }
}
