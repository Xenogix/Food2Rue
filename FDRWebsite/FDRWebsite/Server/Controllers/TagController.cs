using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class TagController : CRUDController<Tag, int>
    {
        public TagController(IRepositoryBase<Tag, int> repository) : base(repository)
        {
        }
    }
}