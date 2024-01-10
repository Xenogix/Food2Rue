using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;

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
                $"DELETE FROM {TABLE_NAME} WHERE id = @Id",
                new
                {
                    Id = key,
                }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Image>> GetAsync()
        {
            return await connection.QueryAsync<Image>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id
                ;");
        }

        public async Task<Image?> GetAsync(int key)
        {
            IEnumerable<Image> temps = await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Image>> GetAsync(IFilter<Image> modelFilter)
        {
            return await connection.QueryAsync<Image>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id
                WHERE {modelFilter.GetFilterSQL}
                ;");
        }

        public async Task<int> InsertAsync(Image model)
        {
            var Transaction = connection.BeginTransaction();
            int idmedia = await connection.QueryFirstAsync<int>(
                @$"INSERT INTO media (url_source) VALUES 
                (@URL_Source) RETURNING id",
                new
                {
                    URL_Source = model.URL_Source
                },
                Transaction);
            await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (id) VALUES ({idmedia}) RETURNING id",
                new { },
                Transaction);
            Transaction.Commit();
            return idmedia;
        }

        public async Task<bool> UpdateAsync(int key, Image model)
        {
            var row = await connection.ExecuteAsync(
                @$"UPDATE media SET url_source = @URL_Source WHERE id = @Id",
            new
            {
                URL_Source = model.URL_Source,
                Id = key
            });

            return row > 0;
        }

    }
}
