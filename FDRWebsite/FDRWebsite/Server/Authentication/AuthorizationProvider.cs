using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using System.Security.Claims;

namespace FDRWebsite.Server.Authentication
{
    public class AuthorizationProvider
    {
        private readonly IRepositoryBase<Utilisateur, int> utilisateurRepository;
        private readonly IFilter<UtilisateurParameters> filter;

        public AuthorizationProvider(IRepositoryBase<Utilisateur, int> utilisateurRepository, IFilter<UtilisateurParameters> filter) {
            this.utilisateurRepository = utilisateurRepository;
            this.filter = filter;
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
                new Claim(UserClaimTypes.ID, user.ID.ToString())
            };
        }

        private async Task<Utilisateur?> GetUserFromLogin(string email, string password)
        {
            var parameters = new UtilisateurParameters()
            {
                Email = email,
                Password = PasswordHashService.CreateHash(email, password)
            };

            filter.SetParameters(parameters);

            return (await utilisateurRepository.GetAsync(filter)).SingleOrDefault();
        }
    }
}
