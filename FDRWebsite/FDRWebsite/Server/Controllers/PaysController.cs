using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class PaysController : CRUDController<Pays, int>
    {
        public PaysController(IRepositoryBase<Pays, int> repository) : base(repository)
        {
        }
    }
}
