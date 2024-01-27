using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Controllers
{
    public class PaysController : ReadonlyController<Pays, string, EmptyFilterParameters>
    {
        public PaysController(IReadonlyRepositoryBase<Pays, string> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }
}
