using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class AllergeneController : CRUDController<Allergene, int>
    {
        public AllergeneController(IRepositoryBase<Allergene, int> repository) : base(repository)
        {
        }
    }
}