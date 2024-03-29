﻿using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Server.Abstractions.Repositories
{
    public interface IRepositoryBase<ModelType, KeyType>
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

        /// <summary>
        /// Insert a new <see cref="ModelType"/> in the physical model 
        /// </summary>
        /// <param name="model"><see cref="ModelType"/> to insert in the physical model</param>
        /// <returns>The key that was assigend with the <see cref="ModelType"/> if it was not specified</returns>
        Task<KeyType?> InsertAsync(ModelType model);

        /// <summary>
        /// Update the <see cref="ModelType"/> with the given key
        /// </summary>
        /// <param name="key">Key of the <see cref="ModelType"/> to update</param>
        /// <param name="model">Values that will override the current <see cref="ModelType"/> values</param>
        /// <returns>Returns true if the operation was successfull or false if an error occured or no <see cref="ModelType"/> exists with the given key</returns>
        Task<bool> UpdateAsync(KeyType key, ModelType model);

        /// <summary>
        /// Delete the <see cref="ModelType"/> with the given key from the physical model
        /// </summary>
        /// <param name="key">Key of the <see cref="ModelType"/> to delete</param>
        /// <returns>Returns true if the operation was successfull or false if an error occured or no <see cref="ModelType"/> exists with the given key</returns>
        Task<bool> DeleteAsync(KeyType key);
    }
}
