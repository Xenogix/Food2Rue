using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using static Azure.Core.HttpHeader;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return await connection.QueryAsync<Publication, Video, string[], string[], Publication>(
                $@"SELECT publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, COUNT(DISTINCT aime_publication_utilisateur.fk_utilisateur) AS aime, COUNT(DISTINCT publication_parent.id) AS commentaires, video.id, mediavid.url_source, 
                array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags 
                FROM publication
                LEFT JOIN media AS mediavid ON mediavid.id = publication.id
                LEFT JOIN video ON video.id = mediavid.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                LEFT JOIN publication AS publication_parent ON publication_parent.fk_parent = publication.id
                LEFT JOIN publication_image ON publication_image.fk_publication = publication.id
                LEFT JOIN media AS mediaim ON mediaim.id = publication_image.fk_image
                LEFT JOIN image ON mediaim.id = image.id
                LEFT JOIN publication_tag ON publication_tag.fk_publication = publication.id
                LEFT JOIN tag ON tag.id = publication_tag.fk_tag
                GROUP BY publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, video.id, mediavid.url_source;",
                (Publication, Video, images, tags) =>
                {
                    Publication.Video = Video;
                    Publication.Images = images[0].IsNullOrEmpty()?null:images.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Publication.Tags = tags[0].IsNullOrEmpty() ? null : tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    return Publication;
                },
                splitOn: "id,id,images,tags")
                ;

        }


        public async Task<Publication?> GetAsync(int key)
        {
            IEnumerable<Publication> Publications = await connection.QueryAsync<Publication, Video, string[], string[], Publication>(
                $@"SELECT publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, COUNT(DISTINCT aime_publication_utilisateur.fk_utilisateur) AS aime, COUNT(DISTINCT publication_parent.id) AS commentaires, video.id, mediavid.url_source, 
                array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags 
                FROM publication
                LEFT JOIN media AS mediavid ON mediavid.id = publication.id
                LEFT JOIN video ON video.id = mediavid.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                LEFT JOIN publication AS publication_parent ON publication_parent.fk_parent = publication.id
                LEFT JOIN publication_image ON publication_image.fk_publication = publication.id
                LEFT JOIN media AS mediaim ON mediaim.id = publication_image.fk_image
                LEFT JOIN image ON mediaim.id = image.id
                LEFT JOIN publication_tag ON publication_tag.fk_publication = publication.id
                LEFT JOIN tag ON tag.id = publication_tag.fk_tag
                WHERE publication.id = {key}
                GROUP BY publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, video.id, mediavid.url_source
                ;",
                (Publication, Video, images, tags) =>
                {
                    Publication.Video = Video;
                    Publication.Images = images[0].IsNullOrEmpty() ? null : images.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Publication.Tags = tags[0].IsNullOrEmpty() ? null : tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    return Publication;
                },
                splitOn: "id,id,images,tags")
                ;
            return Publications.FirstOrDefault();
        }

        public async Task<IEnumerable<Publication>> GetAsync(IFilter filter)
        {
            IEnumerable<Publication> Publications = await connection.QueryAsync<Publication, Video, string[], string[], Publication>(
                $@"SELECT publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, COUNT(DISTINCT aime_publication_utilisateur.fk_utilisateur) AS aime, COUNT(DISTINCT publication_parent.id) AS commentaires, video.id, mediavid.url_source, 
                array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags 
                FROM publication
                LEFT JOIN media AS mediavid ON mediavid.id = publication.id
                LEFT JOIN video ON video.id = mediavid.id
                LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_publication = publication.id
                LEFT JOIN publication AS publication_parent ON publication_parent.fk_parent = publication.id
                LEFT JOIN publication_image ON publication_image.fk_publication = publication.id
                LEFT JOIN media AS mediaim ON mediaim.id = publication_image.fk_image
                LEFT JOIN image ON mediaim.id = image.id
                LEFT JOIN publication_tag ON publication_tag.fk_publication = publication.id
                LEFT JOIN tag ON tag.id = publication_tag.fk_tag
                WHERE {filter.GetFilterSQL()}
                GROUP BY publication.id, publication.texte, publication.date_publication, publication.fk_parent, publication.fk_utilisateur, publication.fk_recette, video.id, mediavid.url_source
                ;",
                (Publication, Video, images, tags) =>
                {
                    Publication.Video = Video;
                    Publication.Images = images[0].IsNullOrEmpty() ? null : images.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Publication.Tags = tags[0].IsNullOrEmpty() ? null : tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    return Publication;
                },
                splitOn: "id,id,images,tags")
                ;
            return Publications;
        }

        public async Task<int> InsertAsync(Publication model)
        {
            ImageRepository imageRepository = new ImageRepository(connection);
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                int? video = model.Video?.ID;
                int id = await connection.QueryFirstAsync<int>(
                    $@"INSERT INTO {TABLE_NAME} (texte, date_publication, fk_parent, fk_utilisateur, fk_recette, fk_video) VALUES 
                    (@Texte, @Date_Publication, @Fk_Parent, @Fk_Utilisateur, @Fk_Recette, @Fk_Video) RETURNING id",
                    new
                    {
                        Texte = model.Texte,
                        Date_Publication = model.Date_Publication,
                        Fk_Parent = model.Parent,
                        Fk_Utilisateur = model.FK_Utilisateur,
                        Fk_Recette = model.Recette,
                        Fk_Video = video
                    },
                    transaction);

                if(model.Tags != null)
                {
                    var publicationTagRepository = new ObjectTagRepository(connection, "publication_tag", "fk_publication");
                    await publicationTagRepository.InsertAsync(new ObjectTag { ID = id, Tags = model.Tags }, transaction);
                }
                if (model.Images != null)
                {
                    var objectImageRepository = new ObjectImageRepository(connection, "publication_image", "fk_publication");
                    await objectImageRepository.InsertAsync(new ObjectImage { ID = id, Images = model.Images }, transaction);
                }

                if (id == 0)
                {
                    transaction.Rollback();
                    return id;
                }
                transaction.Commit();
                return id;
            }
            catch
            {
                transaction.Rollback();
                return 0;
            }
        }

        public async Task<bool> UpdateAsync(int key, Publication model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            ImageRepository imageRepository = new ImageRepository(connection);
            TagRepository tagRepository = new TagRepository(connection);
            ObjectTagRepository publicationTagRepository = new ObjectTagRepository(connection, "publication_tag", "fk_publication");
            var objectImageRepository = new ObjectImageRepository(connection, "publication_image", "fk_publication");
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                int? video = model.Video?.ID;
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
                        Fk_Video = video,
                        Id = key
                    },
                    transaction);


                if (!await publicationTagRepository.UpdateAsync(key, new ObjectTag { ID = key, Tags = model.Tags }, transaction))
                {
                    throw new System.Exception("Error while updating publication tags");
                }

                if (!await objectImageRepository.UpdateAsync(key, new ObjectImage { ID = key, Images = model.Images}, transaction))
                {
                    throw new System.Exception("Error while updating publication images");
                }

                if (row == 0)
                    transaction.Rollback();
                else
                    transaction.Commit();

                return row > 0;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }


    }
}
