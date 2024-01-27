using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FDRWebsite.Client.Authentication
{
    public class LocalAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationService authenticationService;

        private readonly AuthenticationState defaultState = new(new());

        public LocalAuthenticationStateProvider(AuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Check if the user is authenticated (e.g., if a JWT token exists)
            var tokenString = await authenticationService.GetTokenAsync();

            if (tokenString != null)
            {
                var handler = new JwtSecurityTokenHandler();

                if (!handler.CanReadToken(tokenString))
                    return defaultState;

                var token = handler.ReadJwtToken(tokenString);

                var identity = new ClaimsIdentity(token.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }

            return defaultState;
        }

    }
}