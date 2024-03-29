using Blazored.LocalStorage;
using FDRWebsite.Client.Abstractions.Clients;
using FDRWebsite.Client.Authentication;
using FDRWebsite.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.IdentityModel.Logging;

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
            builder.Services.AddScoped<AuthenticationStateProvider, LocalAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ImageService>();
            builder.Services.AddScoped<PostService>();

            IdentityModelEventSource.ShowPII = true;

            builder.Services.AddRestClients(builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }
    }
}