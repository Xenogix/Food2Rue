using FDRWebsite.Shared.Abstraction;
using Refit;

namespace FDRWebsite.Client.Clients
{
    [Headers("Content-Type: application/json",
             "Authorization: Bearer")]
    public interface ICRUDApiClient<ModelType, KeyType, ParametersType>
    {
        [Get("/")]
        Task<IEnumerable<ModelType>> GetAsync();

        [Get("/{key}")]
        Task<ModelType> GetAsync(KeyType key);

        [Post("/filter")]
        Task<IEnumerable<ModelType>> PostAsync([Body] ParametersType parameters);

        [Post("/")]
        Task<KeyType> InsertAsync([Body] ModelType modelType);

        [Put("/{key}")]
        Task<bool> UpdateAsync(KeyType key, [Body] ModelType modelType);

        [Delete("/{key}")]
        Task<bool> DeleteAsync(KeyType key);
    }
}
