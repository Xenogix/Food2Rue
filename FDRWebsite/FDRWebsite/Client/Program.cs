using FDRWebsite.Client.Clients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace FDRWebsite.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddAuthorizationCore();

            // Adding Microsoft Identity authentication
            builder.Services.AddMsalAuthentication(azureOptions => {
                builder.Configuration.Bind("AzureAd", azureOptions.ProviderOptions.Authentication);
            });

            // Adding Google OAuth2 authentication
            /*
            builder.Services.AddMsalAuthentication(googleOptions => {
                builder.Configuration.Bind("GoogleAuth", googleOptions.ProviderOptions.Authentication);
            });
            */

            builder.Services.AddRestClients(builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }
    }
}