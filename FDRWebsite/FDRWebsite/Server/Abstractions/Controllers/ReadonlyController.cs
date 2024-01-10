using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FDRWebsite.Server.Abstractions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ReadonlyController<ModelType, KeyType> : ControllerBase
    where ModelType : IIdentifiable<KeyType>
    where KeyType : IEquatable<KeyType>
    {
        private readonly IRepositoryBase<ModelType, KeyType> repository;

        public ReadonlyController(IRepositoryBase<ModelType, KeyType> repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IEnumerable<ModelType>> GetAsync()
        {
            return await repository.GetAsync();
        }

        [HttpGet("{key}")]
        public async Task<ModelType?> GetAsync(KeyType key)
        {
            return await repository.GetAsync(key);
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<ModelType?>> GetAsync(IFilter<ModelType> modelFilter)
        {
            return await repository.GetAsync(modelFilter);
        }
    }
}
