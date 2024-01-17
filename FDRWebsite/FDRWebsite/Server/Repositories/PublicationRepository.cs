using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;
using System.Text.RegularExpressions;

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
            IEnumerable<Publication> Publications = await connection.QueryAsync<Publication, Video, Publication>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, COUNT(aime_publication_utilisateur.fk_utilisateur) AS aime, video.id, media.url_source FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                LEFT JOIN video ON video.id = media.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, media.id, media.url_source;",
                (Publication, Video) =>
                {
                    Publication.Video = Video;
                    return Publication;
                },
                splitOn: "id,id")
                ;
            foreach (Publication Publication in Publications)
            {
                Publication.Images = await connection.QueryAsync<Image>(
                    $@"SELECT id, url_source FROM image
                INNER JOIN media ON media.id = image.id
                INNER JOIN publication_image ON publication_image.fk_image = image.id
                INNER JOIN publication ON publication.id = publication_image.fk_publication
                WHERE publication.id = @Id;",
                    new
                    {
                        Id = Publication.ID
                    });
            }
            return Publications;
        }


        public async Task<Publication?> GetAsync(int key)
        {
            Publication Publication = (Publication)await connection.QueryAsync<Publication, Video, Publication>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, COUNT(aime_publication_utilisateur.fk_utilisateur) AS aime, video.id, media.url_source FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                LEFT JOIN video ON video.id = media.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                WHERE {TABLE_NAME}.id = @Id
                GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, media.id, media.url_source;",
                (Publication, Video) =>
                {
                    Publication.Video = Video;
                    return Publication;
                },
                splitOn: "id,id")
                ;
            Publication.Images = await connection.QueryAsync<Image>(
                $@"SELECT id, url_source FROM image
                INNER JOIN media ON media.id = image.id
                INNER JOIN publication_image ON publication_image.fk_image = image.id
                INNER JOIN publication ON publication.id = publication_image.fk_publication
                WHERE publication.id = @Id;",
                new
                {
                    Id = Publication.ID
                });
            return Publication;
        }

        public async Task<IEnumerable<Publication>> GetAsync(IFilter<Publication> modelFilter)
        {
            IEnumerable<Publication> Publications = await connection.QueryAsync<Publication, Video, Publication>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, COUNT(aime_publication_utilisateur.fk_utilisateur) AS aime, video.id, media.url_source FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                LEFT JOIN video ON video.id = media.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                WHERE {modelFilter.GetFilterSQL}
                GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.texte, {TABLE_NAME}.date_publication, {TABLE_NAME}.fk_parent, {TABLE_NAME}.fk_utilisateur, {TABLE_NAME}.fk_recette, media.id, media.url_source;",
                (Publication, Video) =>
                {
                    Publication.Video = Video;
                    return Publication;
                },
                splitOn: "id,id")
                ;
            foreach (Publication Publication in Publications)
            {
                Publication.Images = await connection.QueryAsync<Image>(
                    $@"SELECT id, url_source FROM image
                INNER JOIN media ON media.id = image.id
                INNER JOIN publication_image ON publication_image.fk_image = image.id
                INNER JOIN publication ON publication.id = publication_image.fk_publication
                WHERE publication.id = @Id;",
                    new
                    {
                        Id = Publication.ID
                    });
            }
            return Publications;
        }

        public async Task<int> InsertAsync(Publication model)
        {
            ImageRepository imageRepository = new ImageRepository(connection);
            IDbTransaction transaction = connection.BeginTransaction();

            int id = await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (texte, date_publication, fk_parent, fk_utilisateur, fk_recette, fk_video) VALUES 
                (@Texte, @Date_Publication, @Fk_Parent, @Fk_Utilisateur, @Fk_Recette, @Fk_Video)",
                new
                {
                    Texte = model.Texte,
                    Date_Publication = model.Date_Publication,
                    Fk_Parent = model.Parent,
                    Fk_Utilisateur = model.Utilisateur,
                    Fk_Recette = model.Recette,
                    Fk_Video = model.Video.ID
                },
                transaction);
            string tempValues = "";
            foreach (Image Image in model.Images)
            {
                if(await imageRepository.InsertAsync(Image)!=0)
                    tempValues = $"({id}, {Image.ID}),";
            }

            await connection.QueryFirstAsync<int>(
                $@"INSERT INTO publication_image (fk_publication, fk_image) VALUES 
                    @Values",
                new { Values = tempValues },
                transaction);
            return id;
        }

        public async Task<bool> UpdateAsync(int key, Publication model)
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
                    Fk_Utilisateur = model.Utilisateur,
                    Fk_Recette = model.Recette,
                    Fk_Video = model.Video.ID,
                    Id = key
                });
            return row > 0;
        }


    }
}
