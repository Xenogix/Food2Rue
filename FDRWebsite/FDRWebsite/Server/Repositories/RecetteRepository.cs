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
            return await connection.QueryAsync<Recette, Video, Pays, Recette>(
                @$"SELECT {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur 
                    video.id, video.url_source, 
                    pays.id, pays.sigle, pays.nom 
                FROM {TABLE_NAME}
                LEFT media ON {TABLE_NAME}.fk_video = media.id
                INNER JOIN video ON media.id = video.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                ;",
                (Recette, Video, Pays) =>
                {
                    Recette.Video = Video;
                    Recette.Pays = Pays;
                    return Recette;
                },
                splitOn: "id,id,id"
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
                @$"SELECT {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur FROM {TABLE_NAME}
                LEFT media ON {TABLE_NAME}.fk_video = media.id
                INNER JOIN video ON media.id = video.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                WHERE @Filter;",
                new { Filter = modelFilter.GetFilterSQL() }
            );
        }

        public async Task<int> InsertAsync(Recette model)
        {
            return await connection.QueryFirstAsync<int>(
            @$"INSERT INTO {TABLE_NAME} (nom, temps_preparation, temps_cuisson, temps_repos, date_creation, etape, fk_utilisateur, fk_video, fk_pays) VALUES 
                (@Nom, @Temps_Preparation, @Temps_Cuisson, @Temps_Repos, @Date_Creation, @Etape, @Fk_Utilisateur, @Video, @Pays) RETURNING id;
                ",
                new { 
                    Nom = model.Nom,
                    Temps_Preparation = model.Temps_Preparation,
                    Temps_Cuisson = model.Temps_Cuisson,
                    Temps_Repos = model.Temps_Repos,
                    Date_Creation = model.Date_Creation,
                    Etape = model.Etape,
                    Fk_Utilisateur = model.Fk_Utilisateur,
                    Video = model.Video.ID,
                    Pays = model.Pays.ID
                });
        }

        public async Task<bool> UpdateAsync(int key, Recette model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
            @$"UPDATE {TABLE_NAME} SET 
            nom = @Nom, 
            temps_preparation = @Temps_Preparation, 
            temps_cuisson = @Temps_Cuisson, 
            temps_repos = @Temps_Repos, 
            date_creation = @Date_Creation, 
            etape = @Etape, 
            Fk_Utilisateur = @Fk_Utilisateur
            WHERE id = @Id;",
            new
            {
                Nom = model.Nom,
                Temps_Preparation = model.Temps_Preparation,
                Temps_Cuisson = model.Temps_Cuisson,
                Temps_Repos = model.Temps_Repos,
                Date_Creation = model.Date_Creation,
                Etape = model.Etape,
                Fk_Utilisateur = model.Fk_Utilisateur,
                Id = key
            });

            return row > 0;
        }
    }
}
