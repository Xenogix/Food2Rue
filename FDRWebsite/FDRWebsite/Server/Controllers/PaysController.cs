using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class PaysController : ReadonlyController<Pays, string>
    {
        public PaysController(IReadonlyRepositoryBase<Pays, string> repository) : base(repository)
        {
        }
    }
}
