using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class AllergeneRepository : IRepositoryBase<Allergene, String>
    {
        private const string TABLE_NAME = "allergene";

        private readonly NpgsqlConnection connection;

        public AllergeneRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(String key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE nom = @Id",
                new
                {
                    Id = key,
                }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Allergene>> GetAsync()
        {
            return await connection.QueryAsync<Allergene>(
                @$"SELECT nom AS id FROM {TABLE_NAME};"
            );
        }

        public async Task<Allergene?> GetAsync(String key)
        {
            IEnumerable<Allergene> temps = await connection.QueryAsync<Allergene>(
                @$"SELECT nom AS id FROM {TABLE_NAME} WHERE nom = @ID;",
                new { ID = key }
            );
            return temps.FirstOrDefault();
        }

        public async Task<IEnumerable<Allergene>> GetAsync(IFilter<Allergene> modelFilter)
        {
            return await connection.QueryAsync<Allergene>(
                @$"SELECT nom AS id FROM {TABLE_NAME} WHERE @Filter;",
                new { Filter = modelFilter.GetFilterSQL() }
            );
        }

        public async Task<String> InsertAsync(Allergene model)
        {
            return await connection.QueryFirstAsync<String>(
            @$"INSERT INTO {TABLE_NAME} (nom) VALUES 
                (@Nom) RETURNING nom;
                ",
                new { Nom = model.ID });
        }

        public async Task<bool> UpdateAsync(String key, Allergene model)
        {
            if (!model.ID.Equals(0) && !key.Equals(model.ID))
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
            @$"UPDATE {TABLE_NAME} SET 
                nom = @ID
                WHERE nom = @Id;",
            new { ID = model.ID });

            return row > 0;
        }
    }
}
