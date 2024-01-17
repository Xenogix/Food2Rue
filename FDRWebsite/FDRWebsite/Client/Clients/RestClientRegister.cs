﻿using Refit;

namespace FDRWebsite.Client.Clients
{
    public static class RestClientRegister
    {
        public static IServiceCollection AddRestClients(this IServiceCollection services, string baseUrl)
        {
            services.AddRefitClient(typeof(IPublicationClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/publication"); });
            services.AddRefitClient(typeof(IUtilisateurClient)).ConfigureHttpClient(x => { x.BaseAddress = new Uri($"{baseUrl}api/user"); });

            return services;
        }
    }
}
