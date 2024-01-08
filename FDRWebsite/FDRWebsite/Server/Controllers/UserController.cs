using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class UserController : CRUDController<User, int>
    {
        public UserController(IRepositoryBase<User, int> repository) : base(repository)
        {
        }
    }
}
