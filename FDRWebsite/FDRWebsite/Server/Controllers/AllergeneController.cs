using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class AllergeneController : CRUDController<Allergene, string, EmptyFilterParameters>
    {
        public AllergeneController(IRepositoryBase<Allergene, string> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }
}