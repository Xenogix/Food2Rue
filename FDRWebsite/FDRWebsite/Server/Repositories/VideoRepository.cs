using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;
using System.Transactions;

namespace FDRWebsite.Server.Repositories
{
    public class VideoRepository : IRepositoryBase<Video, int>
    {
        private const string TABLE_NAME = "video";

        private readonly NpgsqlConnection connection;

        public VideoRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Video>> GetAsync()
        {
            return await connection.QueryAsync<Video>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id
                ;");
        }

        public async Task<Video?> GetAsync(int key)
        {
            IEnumerable<Video> temps = await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Video>> GetAsync(IFilter filter)
        {
            return await connection.QueryAsync<Video>(
                $@"SELECT {TABLE_NAME}.id, url_source FROM {TABLE_NAME}
                INNER JOIN media ON media.id = {TABLE_NAME}.id
                WHERE {filter.GetFilterSQL}
                ;");
        }
        public async Task<int> InsertAsync(Video model, IDbTransaction Transaction)
        {
            int idmedia = await connection.QueryFirstAsync<int>(
                @$"INSERT INTO media (url_source) VALUES 
                (@URL_Source) RETURNING id",
                new
                {
                    URL_Source = model.URL_Source
                },
                Transaction);
            await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (id) VALUES (@Idmedia) RETURNING id",
                new { Idmedia = idmedia},
                Transaction);
            return idmedia;
        }
        public async Task<int> InsertAsync(Video model)
        {
            var Transaction = connection.BeginTransaction();
            int idmedia = await InsertAsync(model, Transaction);
            if (idmedia != -1)
            {
                Transaction.Commit();
                return idmedia;
            }
            Transaction.Rollback();
            return -1;
        }

        public async Task<bool> UpdateAsync(int key, Video model)
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