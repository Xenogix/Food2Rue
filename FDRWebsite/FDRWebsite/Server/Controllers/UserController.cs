using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class UserController : CRUDController<Utilisateur, int>
    {
        public UserController(IRepositoryBase<Utilisateur, int> repository) : base(repository)
        {
        }
    }
}
