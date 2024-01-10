using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class PaysRepository : IReadonlyRepositoryBase<Pays, int>
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
                @$"SELECT id, sigle, nom FROM {TABLE_NAME};"
            );
        }

        public async Task<Pays?> GetAsync(int key)
        {
            IEnumerable<Pays> temps = (IEnumerable<Pays>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Pays>> GetAsync(IFilter<Pays> modelFilter)
        {
            return await connection.QueryAsync<Pays>(
                @$"SELECT id, sigle, nom FROM {TABLE_NAME} WHERE {modelFilter.GetFilterSQL()};"
            );
        }

    }
}
