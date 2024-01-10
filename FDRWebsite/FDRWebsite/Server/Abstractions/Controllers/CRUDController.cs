using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FDRWebsite.Server.Abstractions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class CRUDController<ModelType, KeyType> : ControllerBase
        where ModelType : IIdentifiable<KeyType>
        where KeyType : IEquatable<KeyType>
    {
        private readonly IRepositoryBase<ModelType, KeyType> repository;

        public CRUDController(IRepositoryBase<ModelType, KeyType> repository)
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

        [HttpPost]
        public async Task<KeyType?> PostAsync(ModelType model)
        {
            return await repository.InsertAsync(model);
        }

        [HttpPut("{key}")]
        public async Task<bool> PutAsync(KeyType key, ModelType model)
        {
            return await repository.UpdateAsync(key, model);
        }

        [HttpDelete("{key}")]
        public async Task<bool> DeleteAsync(KeyType key)
        {
            return await repository.DeleteAsync(key);
        }
    }
}
