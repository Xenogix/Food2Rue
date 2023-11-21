using Refit;

namespace FDRWebsite.Client.Abstractions.Clients
{
    public interface IReadonlyApiClient<ModelType, KeyType>
    {
        [Get("/")]
        Task<IEnumerable<ModelType>> GetAsync();

        [Get("/{key}")]
        Task<ModelType> GetAsync(KeyType key);
    }
}
