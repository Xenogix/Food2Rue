using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class TagRepository : IRepositoryBase<Tag, int>
    {
        private const string TABLE_NAME = "tag";

        private readonly NpgsqlConnection connection;

        public TagRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Tag>> GetAsync()
        {
            return await connection.QueryAsync<Tag>(
                @$"SELECT id, nom FROM {TABLE_NAME};"
            );
        }

        public async Task<Tag?> GetAsync(int key)
        {
            IEnumerable<Tag> temps = (IEnumerable<Tag>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Tag>> GetAsync(IFilter filter)
        {
            return await connection.QueryAsync<Tag>(
                @$"SELECT id, nom FROM {TABLE_NAME} WHERE @Filter;",
                new { Filter = filter.GetFilterSQL()}
            );
        }

        public async Task<int> InsertAsync(Tag model)
        {
            return await connection.QueryFirstAsync<int>(
            @$"INSERT INTO { TABLE_NAME } (nom) VALUES 
                (@Nom) RETURNING id;
                ",
                new { Nom = model.Nom });
        }

        public async Task<bool> UpdateAsync(int key, Tag model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
            @$"UPDATE { TABLE_NAME } SET 
                nom = @Nom
                WHERE id = @Id;",
            new {Nom = model.Nom, Id = key});

            return row > 0;
        }
    }
}
