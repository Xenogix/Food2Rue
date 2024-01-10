using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class RecetteRepository : IRepositoryBase<Recette, int>
    {
        private const string TABLE_NAME = "Recette";

        private readonly NpgsqlConnection connection;

        public RecetteRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Recette>> GetAsync()
        {
            return await connection.QueryAsync<Recette, User, Video, Pays, Recette>(
                @$"SELECT {TABLE_NAME}.*, fk_utilisateur, fk_video, fk_pays FROM {TABLE_NAME}
                LEFT JOIN utilisateur ON fk_utilisateur = utilisateur.id
                LEFT JOIN video ON fk_video = video.id
                LEFT JOIN pays ON fk_pays = pays.id
                ;",
                (Recette, User, Video, Pays) =>
                {
                    Recette.Utilisateur = User;
                    Recette.Video = Video;
                    Recette.Pays = Pays;
                    return Recette;
                },
                splitOn: "id,id"
            );
        }

        public async Task<Recette?> GetAsync(int key)
        {
            IEnumerable<Recette> temps = (IEnumerable<Recette>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Recette>> GetAsync(IFilter<Recette> modelFilter)
        {
            return await connection.QueryAsync<Recette>(
                @$"SELECT id, nom FROM {TABLE_NAME} WHERE @Filter;",
                new { Filter = modelFilter.GetFilterSQL() }
            );
        }

        public async Task<int> InsertAsync(Recette model)
        {
            return await connection.QueryFirstAsync<int>(
            @$"INSERT INTO {TABLE_NAME} (nom) VALUES 
                (@Nom) RETURNING id;
                ",
                new { Nom = model.Nom });
        }

        public async Task<bool> UpdateAsync(int key, Recette model)
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
