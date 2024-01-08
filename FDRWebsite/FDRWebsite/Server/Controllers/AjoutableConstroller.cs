using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class IngredientController : CRUDController<Ingredient, int>
    {
        public IngredientController(IRepositoryBase<Ingredient, int> repository) : base(repository)
        {
        }
    }

    public class UstensileController : CRUDController<Ustensile, int>
    {
        public UstensileController(IRepositoryBase<Ustensile, int> repository) : base(repository)
        {
        }
    }
}