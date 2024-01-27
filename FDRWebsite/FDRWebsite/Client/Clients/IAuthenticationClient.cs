using FDRWebsite.Shared.Models;
using Refit;
using System.IdentityModel.Tokens.Jwt;

namespace FDRWebsite.Client.Clients
{
    [Headers("Content-Type: application/json")]
    public interface IAuthenticationClient
    {
        [Post("/")]
        Task<AuthenticationResponse> AuthenticateAsync(LoginRequest loginRequest);
    }
}
