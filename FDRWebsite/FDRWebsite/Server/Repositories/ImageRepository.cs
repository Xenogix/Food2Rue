using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class ImageRepository : IRepositoryBase<Image, int>
    {
        private const string TABLE_NAME = "image";

        private readonly NpgsqlConnection connection;

        public ImageRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE id = @ID",
                new { ID = key }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Image>> GetAsync()
        {
            return await connection.QueryAsync<Image>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id;");
        }

        public async Task<Image?> GetAsync(int key)
        {
            return await connection.QueryFirstAsync<Image>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id;$
                WHERE {TABLE_NAME}.id = @ID",
                new { ID = key }
            );
        }

        public async Task<IEnumerable<Image>> GetAsync(IFilter filter)
        {
            return await connection.QueryAsync<Image>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id
                WHERE {filter.GetFilterSQL};",
                filter.GetFilterParameters()
                );
        }

        public async Task<int> InsertAsync(Image model)
        {
            // Executed in a transaction as if only one of the insert works the database state could be inconsistent
            using var transaction = connection.BeginTransaction();

            int idMedia = await connection.QueryFirstAsync<int>(
                @$"INSERT INTO media (url_source) VALUES (@URLSource) RETURNING id",
                new { URLSource = model.URL_Source},
                transaction);

            int idImage = await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (id) VALUES (@ID) RETURNING id",
                new { ID = @idMedia },
                transaction);

            transaction.Commit();

            return idMedia;
        }

        public async Task<bool> UpdateAsync(int key, Image model)
        {
            var row = await connection.ExecuteAsync(
                @$"UPDATE media SET url_source = @URLSource, id = @NewID WHERE id = @ID",
                new { URLSource = model.URL_Source, ID = key, NewID = model.ID }
            );

            return row > 0;
        }

    }
}
