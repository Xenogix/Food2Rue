using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Server.Abstractions.Repositories
{
    public interface IReadonlyRepositoryBase<ModelType, KeyType> 
        where ModelType : IIdentifiable<KeyType>
        where KeyType : IEquatable<KeyType>
    {
        /// <summary>
        /// Get every <see cref="ModelType"/> in the physical model
        /// </summary>
        /// <returns>The fetched <see cref="ModelType"/></returns>
        Task<IEnumerable<ModelType>> GetAsync();

        /// <summary>
        /// Get the <see cref="ModelType"/> with the given key or return null
        /// </summary>
        /// <param name="key">Key of the <see cref="ModelType"/> to fetch</param>
        /// <returns>The <see cref="ModelType"/> model with the given key</returns>
        Task<ModelType?> GetAsync(KeyType key);

        /// <summary>
        /// Get every <see cref="ModelType"/> in the physical model that match the given filter
        /// </summary>
        /// <returns>The fetched <see cref="ModelType"/></returns>
        Task<IEnumerable<ModelType>> GetAsync(IFilter filter);
    }
}
