using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class UtilisateursController : CRUDController<Utilisateur, int>
    {
        public UtilisateursController(IRepositoryBase<Utilisateur, int> repository) : base(repository)
        {
        }
    }
}
