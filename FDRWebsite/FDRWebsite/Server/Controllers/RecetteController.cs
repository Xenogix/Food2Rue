using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class RecetteController : CRUDController<Recette, int>
    {
        public RecetteController(IRepositoryBase<Recette, int> repository) : base(repository)
        {
        }
    }
}