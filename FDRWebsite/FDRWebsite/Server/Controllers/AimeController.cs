using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models.Objects;

namespace FDRWebsite.Server.Controllers
{
    public class AimeController : CRUDController<Aime, AimeKey, EmptyFilterParameters>
    {
        public AimeController(IRepositoryBase<Aime, AimeKey> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }

    }
}
