using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Server.Repositories;
using FDRWebsite.Server.Repositories.Filters;
using FDRWebsite.Shared.Models;
using System.Security.Claims;

namespace FDRWebsite.Server.Authentication
{
    public class AuthorizationProvider
    {
        private readonly IRepositoryBase<Utilisateur, int> utilisateurRepository;

        public AuthorizationProvider(IRepositoryBase<Utilisateur, int> utilisateurRepository) {
            this.utilisateurRepository = utilisateurRepository;
        }

        public async Task<IEnumerable<Claim>?> GetUserClaims(string? email, string? password)
        {
            if (email == null || password == null) return null;

            var user = await GetUserFromLogin(email, password);

            if (user == null) return null;

            return new Claim[]{
                new Claim(ClaimTypes.Name, user.Pseudo),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User"),
            };
        }

        private async Task<Utilisateur?> GetUserFromLogin(string email, string password)
        {
            var filter = new UtilisateurFilter()
            {
                Email = email,
                Password = PasswordHashService.CreateHash(email, password)
            };

            return (await utilisateurRepository.GetAsync(filter)).SingleOrDefault();
        }
    }
}
