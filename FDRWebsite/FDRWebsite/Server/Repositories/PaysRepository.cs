using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class PaysRepository : IReadonlyRepositoryBase<Pays, string>
    {
        private const string TABLE_NAME = "pays";

        private readonly NpgsqlConnection connection;

        public PaysRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Pays>> GetAsync()
        {
            return await connection.QueryAsync<Pays>(
                @$"SELECT sigle, nom FROM {TABLE_NAME};"
            );
        }

        public async Task<Pays?> GetAsync(string key)
        {
            var parameters = new { key = key };
            return await connection.QueryFirstAsync<Pays>(@$"SELECT sigle, nom FROM {TABLE_NAME} WHERE sigle = @key", parameters);
        }

        public async Task<IEnumerable<Pays>> GetAsync(IFilter<Pays> modelFilter)
        {
            return await connection.QueryAsync<Pays>(
                @$"SELECT sigle, nom FROM {TABLE_NAME} WHERE {modelFilter.GetFilterSQL()};"
            );
        }

    }
}
