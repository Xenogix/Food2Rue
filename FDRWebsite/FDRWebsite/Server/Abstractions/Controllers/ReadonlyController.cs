using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FDRWebsite.Server.Abstractions.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ReadonlyController<ModelType, KeyType, ParametersType> : ControllerBase
        where ModelType : IIdentifiable<KeyType>
        where KeyType : IEquatable<KeyType>
    {
        private readonly IReadonlyRepositoryBase<ModelType, KeyType> repository;
        private readonly IFilter<ParametersType> filter;

        public ReadonlyController(IReadonlyRepositoryBase<ModelType, KeyType> repository, IFilter<ParametersType> filter)
        {
            this.repository = repository;
            this.filter = filter;
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

        [HttpPost("filter")]
        public async Task<IEnumerable<ModelType?>> PostAsync(ParametersType parameters)
        {
            filter.SetParameters(parameters);
            return await repository.GetAsync(filter);
        }
    }
}
