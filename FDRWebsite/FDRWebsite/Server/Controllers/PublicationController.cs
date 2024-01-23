using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Controllers
{
    public class PublicationController : CRUDController<Publication, int>
    {
        public PublicationController(IRepositoryBase<Publication, int> repository) : base(repository)
        {
        }
    }
}
