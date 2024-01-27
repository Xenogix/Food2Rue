using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class AllergeneRepository : IRepositoryBase<Allergene, string>
    {
        private const string TABLE_NAME = "allergene";

        private readonly NpgsqlConnection connection;

        public AllergeneRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(string key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE nom = @ID",
                new { ID = key }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Allergene>> GetAsync()
        {
            return await connection.QueryAsync<Allergene>(
                @$"SELECT nom FROM {TABLE_NAME};"
            );
        }

        public async Task<Allergene?> GetAsync(string key)
        {
            return await connection.QueryFirstAsync<Allergene>(
                @$"SELECT nom AS id FROM {TABLE_NAME} WHERE nom = @ID;",
                new { ID = key }
            );
        }

        public async Task<IEnumerable<Allergene>> GetAsync(IFilter<Allergene> modelFilter)
        {
            return await connection.QueryAsync<Allergene>(
                @$"SELECT nom AS id FROM {TABLE_NAME} WHERE {modelFilter.GetFilterSQL()};",
                modelFilter.GetFilterParameters()
            );
        }

        public async Task<string> InsertAsync(Allergene model)
        {
            return await connection.QueryFirstAsync<string>(
            @$"INSERT INTO {TABLE_NAME} (nom) VALUES (@ID) RETURNING nom;",
            new { ID = model.ID });
        }

        public async Task<bool> UpdateAsync(string key, Allergene model)
        {
            var row = await connection.ExecuteAsync(
            @$"UPDATE {TABLE_NAME} SET nom = @NewID WHERE nom = @ID;",
            new { ID = model.ID, NewID = model.ID });

            return row > 0;
        }
    }
}
