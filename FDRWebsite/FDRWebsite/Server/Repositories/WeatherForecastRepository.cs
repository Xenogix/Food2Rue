using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class WeatherForecastRepository : IRepositoryBase<WeatherForecast, int>
    {
        private const string TABLE_NAME = "weather";

        private readonly NpgsqlConnection connection;

        public WeatherForecastRepository(NpgsqlConnection connection) {
            this.connection = connection;
        }

        public async Task<IEnumerable<WeatherForecast>> GetAsync()
        {
            return await connection.QueryAsync<WeatherForecast>(
                $"SELECT * FROM {TABLE_NAME}"
            );
        }

        public async Task<WeatherForecast?> GetAsync(int key)
        {
            return await connection.QueryFirstAsync<WeatherForecast>(
                $"SELECT * FROM {TABLE_NAME} WHERE id = @id",
                new {id = key}
            );
        }

        public async Task<IEnumerable<WeatherForecast>> GetAsync(IFilter<WeatherForecast> modelFilter)
        {
            return await connection.QueryAsync<WeatherForecast>(
                $"SELECT * FROM {TABLE_NAME} WHERE @filter",
                new { filter = modelFilter.GetFilterSQL() }
            );
        }

        public async Task<int> InsertAsync(WeatherForecast model)
        {
            return await connection.QueryFirstAsync<int>(
                $"INSERT INTO {TABLE_NAME} (columnName1, columnName2, columnName3, ...) VALUES (@Value1, @Value2, @Value3, ...)",
                new
                {
                    Value1 = "<ModelProperty1>",
                    Value2 = "<ModelProperty2>",
                    Value3 = "<ModelProperty3>",
                }
            ); ;
        }

        public async Task<bool> UpdateAsync(int key, WeatherForecast model)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"UPDATE {TABLE_NAME} SET columnName1 = @Value1, columnName2 = @Value2, columnName3 = @Value3, ... WHERE id = @Id",
                new
                {
                    Id     = "<ModelId>",
                    Value1 = "<ModelProperty1>",
                    Value2 = "<ModelProperty2>",
                    Value3 = "<ModelProperty3>",
                }
            );

            return affectedRows > 0;
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

    }
}
