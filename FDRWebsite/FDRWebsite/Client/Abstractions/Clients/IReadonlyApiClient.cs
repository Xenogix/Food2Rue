using FDRWebsite.Shared.Abstraction;
using Refit;

namespace FDRWebsite.Client.Abstractions.Clients
{
    public interface IReadonlyApiClient<ModelType, KeyType, ParametersTypes>
    {
        [Get("/")]
        Task<IEnumerable<ModelType>> GetAsync();

        [Get("/{key}")]
        Task<ModelType> GetAsync(KeyType key);

        [Post("/filter")]
        Task<IEnumerable<ModelType>> PostAsync([Body] ParametersTypes parameters);
    }
}
