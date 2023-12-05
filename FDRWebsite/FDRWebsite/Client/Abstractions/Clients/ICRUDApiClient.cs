using Refit;

namespace FDRWebsite.Client.Clients
{
    [Headers("Content-Type: application/json",
             "Authorization: Bearer")]
    public interface ICRUDApiClient<ModelType, KeyType>
    {
        [Get("/")]
        Task<IEnumerable<ModelType>> GetAsync();

        [Get("/{key}")]
        Task<ModelType> GetAsync(KeyType key);

        [Post("/")]
        Task<KeyType> InsertAsync([Body] ModelType modelType);

        [Put("/{key}")]
        Task<bool> UpdateAsync(KeyType key, [Body] ModelType modelType);

        [Delete("/{key}")]
        Task<bool> DeleteAsync(KeyType key);
    }
}
