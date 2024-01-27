using FDRWebsite.Server.Authentication;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FDRWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthorizationProvider provider;
        private readonly IConfiguration configuration;

        public AuthenticationController(AuthorizationProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        [HttpPost]
        public async Task<AuthenticationResponse?> AuthenticateAsync([FromBody] LoginRequest model)
        {
            var claims = await provider.GetUserClaims(model?.Email, model?.Password);
            if (claims == null) return null;

            // Generate JWT token
            var token = new JwtSecurityToken(
                issuer: configuration["JwtOptions:Issuer"],
                audience: configuration["JwtOptions:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtOptions:ExpiresInMinutes"])),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtOptions:Key"]!)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return token
            return new AuthenticationResponse() { Token = tokenString };
        }
    }
}
