using FDRWebsite.Client.Clients;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FDRWebsite.Client.Authentication
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IUtilisateurClient utilisateurClient;

        public LocalAuthenticationStateProvider(IUtilisateurClient utilisateurClient)
        {
            this.utilisateurClient = utilisateurClient;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "My Name"),
            },
            "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
