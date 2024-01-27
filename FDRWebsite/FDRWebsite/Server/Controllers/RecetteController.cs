using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class RecetteController : CRUDController<Recette, int, EmptyFilterParameters>
    {
        public RecetteController(IRepositoryBase<Recette, int> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }
}