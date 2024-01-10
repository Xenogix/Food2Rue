using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class AllergeneRepository : IRepositoryBase<Allergene, int>
    {
        private const string TABLE_NAME = "allergene";

        private readonly NpgsqlConnection connection;

        public AllergeneRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE id = @Id",
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
                @$"SELECT id, nom FROM {TABLE_NAME};"
            );
        }

        public async Task<Allergene?> GetAsync(int key)
        {
            IEnumerable<Allergene> temps = (IEnumerable<Allergene>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Allergene>> GetAsync(IFilter<Allergene> modelFilter)
        {
            return await connection.QueryAsync<Allergene>(
                @$"SELECT id, nom FROM {TABLE_NAME} WHERE @Filter;",
                new { Filter = modelFilter.GetFilterSQL() }
            );
        }

        public async Task<int> InsertAsync(Allergene model)
        {
            return await connection.QueryFirstAsync<int>(
            @$"INSERT INTO {TABLE_NAME} (nom) VALUES 
                (@Nom) RETURNING id;
                ",
                new { Nom = model.Nom });
        }

        public async Task<bool> UpdateAsync(int key, Allergene model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
            @$"UPDATE {TABLE_NAME} SET 
                nom = @Nom
                WHERE id = @Id;",
            new { Nom = model.Nom, Id = key });

            return row > 0;
        }
    }
}
