using FDRWebsite.Server.Abstractions.Controllers;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace FDRWebsite.Server.Controllers
{
    public class IngredientController : CRUDController<Ingredient, int, EmptyFilterParameters>
    {
        public IngredientController(IRepositoryBase<Ingredient, int> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }

    public class UstensileController : CRUDController<Ustensile, int, EmptyFilterParameters>
    {
        public UstensileController(IRepositoryBase<Ustensile, int> repository, IFilter<EmptyFilterParameters> filter) : base(repository, filter)
        {
        }
    }
}