using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;

namespace FDRWebsite.Server.Controllers
{
    public class PublicationController : CRUDController<Publication, int, PublicationParameters>
    {
        public PublicationController(IRepositoryBase<Publication, int> repository, IFilter<PublicationParameters> filter) : base(repository, filter)
        {
        }
    }
}
