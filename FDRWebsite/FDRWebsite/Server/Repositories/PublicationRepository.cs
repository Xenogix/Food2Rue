using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;

namespace FDRWebsite.Server.Repositories
{
    public class PublicationRepository : IRepositoryBase<Publication, int>
    {
        private const string TABLE_NAME = "publication";

        private readonly NpgsqlConnection connection;

        public PublicationRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Publication>> GetAsync()
        {
            return await connection.QueryAsync<Publication, Video, Publication>(
                $@"SELECT * FROM {TABLE_NAME}
                   LEFT JOIN media ON media.id = {TABLE_NAME}.id
                   LEFT JOIN video ON video.id = media.id
                   ;",
                    (publication, video) =>
                    {   
                        publication.Video = video;
                        return publication;
                    },
                    splitOn: "id, id" 
                );
        }


        public async Task<Publication?> GetAsync(int key)
        {
            IEnumerable<Publication> temps = await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Publication>> GetAsync(IFilter<Publication> modelFilter)
        {
            return await connection.QueryAsync<Publication>(
                $@"SELECT id, texte, date_publication, fk_parent, fk_utilisateur, fk_recette, fk_video FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                LEFT JOIN video ON video.id = media.id
                WHERE {modelFilter.GetFilterSQL}
                ;");
        }

        public async Task<int> InsertAsync(Publication model)
        {
            return await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (texte, date_publication, fk_parent, fk_utilisateur, fk_recette, fk_video) VALUES 
                (@Texte, @Date_Publication, @Fk_Parent, @Fk_Utilisateur, @Fk_Recette, @Fk_Video)",
                new
                {
                    Texte = model.Texte,
                    Date_Publication = model.Date_Publication,
                    Fk_Parent = model.Parent,
                    Fk_Utilisateur = model.FK_Utilisateur,
                    Fk_Recette = model.Recette,
                    Fk_Video = model.Video.ID
                });
        }

        public async Task<bool> UpdateAsync(int key, Publication model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
                @$"UPDATE media SET url_source = @URL_Source WHERE id = @Id",
            new
            {
                //URL_Source = model.URL_Source,
                Id = key
            });

            return row > 0;
        }
        public async Task<bool> UpdateAsync(int key, Publication model, Media mediaModel, Video videoModel)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
                @$"UPDATE {TABLE_NAME} SET 
                texte = @Texte, 
                date_publication = @Date_Publication, 
                fk_parent = @Fk_Parent, 
                fk_utilisateur = @Fk_Utilisateur, 
                fk_recette = @Fk_Recette, 
                fk_video = @Fk_Video
                WHERE id = @Id",
                new
                {
                    Texte = model.Texte,
                    Date_Publication = model.Date_Publication,
                    Fk_Parent = model.Parent,
                    Fk_Utilisateur = model.FK_Utilisateur,
                    Fk_Recette = model.Recette,
                    Fk_Video = model.Video.ID,
                    Id = key
                });
            return row > 0;
        }


    }
}
