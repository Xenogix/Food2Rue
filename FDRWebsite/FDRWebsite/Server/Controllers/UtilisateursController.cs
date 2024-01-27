using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class UtilisateursController : CRUDController<Utilisateur, int, UtilisateurParameters>
    {
        public UtilisateursController(IRepositoryBase<Utilisateur, int> repository, IFilter<UtilisateurParameters> filter) : base(repository, filter)
        {
        }
    }
}
