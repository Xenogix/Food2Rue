using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;

namespace FDRWebsite.Server.Repositories
{
    public class RecetteRepository : IRepositoryBase<Recette, int>
    {
        private const string TABLE_NAME = "recette";

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
            return await connection.QueryAsync<Recette, Video, Pays, string[], string[], string[], int[], Recette>(
                @$"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom, 
                        (SUM (DISTINCT note.note) / count(DISTINCT note.note)) AS note,
                        array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                        array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags,
                        array_agg(DISTINCT ingredient.fk_ingredient || ',' || ingredient.quantite) AS ingredients,
                        array_agg(DISTINCT ustensile.fk_ustensile) AS usetensiles
                    FROM {TABLE_NAME}
                    LEFT JOIN video ON {TABLE_NAME}.fk_video = video.id
                    LEFT JOIN media AS mediavid ON video.id = mediavid.id
                    LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                    LEFT JOIN note ON fk_{TABLE_NAME} = {TABLE_NAME}.id
                    LEFT JOIN {TABLE_NAME}_image ON {TABLE_NAME}.id = recette_image.fk_recette
                    LEFT JOIN image ON recette_image.fk_image = image.id
                    LEFT JOIN media AS mediaim ON image.id = mediaim.id
                    LEFT JOIN recette_tag ON {TABLE_NAME}.id = recette_tag.fk_recette
                    LEFT JOIN tag ON recette_tag.fk_tag =  tag.id
                    LEFT JOIN recette_ingredient AS ingredient ON recette.id = ingredient.fk_recette
                    LEFT JOIN recette_ustensile AS ustensile ON recette.id = ustensile.fk_recette
                    GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom
                    ;
                ",
                (Recette, Video, Pays, Images, Tags, Ingredients, Ustensiles) =>
                {
                    Recette.Video = Video;
                    Recette.Pays = Pays;
                    Recette.Images = Images[0].IsNullOrEmpty() ? null : Images.Select(x => x.Split(','))
                                        .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                        .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Recette.Tags = Tags[0].IsNullOrEmpty() ? null : Tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    Recette.Ingredients = Ingredients[0].IsNullOrEmpty() ? null : Ingredients.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1]);
                    Recette.Ustensiles = Ustensiles;
                    return Recette;
                },
                splitOn: "id,id,id,images,tags,ingredients,usetensiles"
            );
        }

        public async Task<Recette?> GetAsync(int key)
        {
            IEnumerable<Recette> recettes = await connection.QueryAsync<Recette, Video, Pays, string[], string[], string[], int[], Recette>(
                @$"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom, 
                        (SUM (DISTINCT note.note) / count(DISTINCT note.note)) AS note,
                        array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                        array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags,
                        array_agg(DISTINCT ingredient.fk_ingredient || ',' || ingredient.quantite) AS ingredients,
                        array_agg(DISTINCT ustensile.fk_ustensile) AS usetensiles
                    FROM {TABLE_NAME}
                    LEFT JOIN video ON {TABLE_NAME}.fk_video = video.id
                    LEFT JOIN media AS mediavid ON video.id = mediavid.id
                    LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                    LEFT JOIN note ON fk_{TABLE_NAME} = {TABLE_NAME}.id
                    LEFT JOIN {TABLE_NAME}_image ON {TABLE_NAME}.id = recette_image.fk_recette
                    LEFT JOIN image ON recette_image.fk_image = image.id
                    LEFT JOIN media AS mediaim ON image.id = mediaim.id
                    LEFT JOIN recette_tag ON {TABLE_NAME}.id = recette_tag.fk_recette
                    LEFT JOIN tag ON recette_tag.fk_tag =  tag.id
                    LEFT JOIN recette_ingredient AS ingredient ON recette.id = ingredient.fk_recette
                    LEFT JOIN recette_ustensile AS ustensile ON recette.id = ustensile.fk_recette
                    WHERE {TABLE_NAME}.id = {key}
                    GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom
                    ;
                ",
                (Recette, Video, Pays, Images, Tags, Ingredients, Ustensiles) =>
                {
                    Recette.Video = Video;
                    Recette.Pays = Pays;
                    Recette.Images = Images[0].IsNullOrEmpty() ? null : Images.Select(x => x.Split(','))
                                        .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                        .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Recette.Tags = Tags[0].IsNullOrEmpty() ? null : Tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    Recette.Ingredients = Ingredients[0].IsNullOrEmpty() ? null : Ingredients.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1]);
                    Recette.Ustensiles = Ustensiles;
                    return Recette;
                },
                splitOn: "id,id,id,images,tags,ingredients,usetensiles"
            );
            return recettes.FirstOrDefault();
        }

        public async Task<IEnumerable<Recette>> GetAsync(IFilter<Recette> modelFilter)
        {
            return await connection.QueryAsync<Recette, Video, Pays, string[], string[], string[], int[], Recette>(
                @$"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom, 
                        (SUM (DISTINCT note.note) / count(DISTINCT note.note)) AS note,
                        array_agg(DISTINCT mediaim.id || ',' ||mediaim.url_source) AS images,
                        array_agg(DISTINCT tag.id || ',' ||tag.nom) AS tags,
                        array_agg(DISTINCT ingredient.fk_ingredient || ',' || ingredient.quantite) AS ingredients,
                        array_agg(DISTINCT ustensile.fk_ustensile) AS usetensiles
                    FROM {TABLE_NAME}
                    LEFT JOIN video ON {TABLE_NAME}.fk_video = video.id
                    LEFT JOIN media AS mediavid ON video.id = mediavid.id
                    LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                    LEFT JOIN note ON fk_{TABLE_NAME} = {TABLE_NAME}.id
                    LEFT JOIN {TABLE_NAME}_image ON {TABLE_NAME}.id = recette_image.fk_recette
                    LEFT JOIN image ON recette_image.fk_image = image.id
                    LEFT JOIN media AS mediaim ON image.id = mediaim.id
                    LEFT JOIN recette_tag ON {TABLE_NAME}.id = recette_tag.fk_recette
                    LEFT JOIN tag ON recette_tag.fk_tag =  tag.id
                    LEFT JOIN recette_ingredient AS ingredient ON recette.id = ingredient.fk_recette
                    LEFT JOIN recette_ustensile AS ustensile ON recette.id = ustensile.fk_recette
                    WHERE {TABLE_NAME}.id = {modelFilter.GetFilterSQL}
                    GROUP BY {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.temps_preparation, {TABLE_NAME}.temps_cuisson, {TABLE_NAME}.temps_repos, {TABLE_NAME}.date_creation, {TABLE_NAME}.etape, {TABLE_NAME}.Fk_Utilisateur,
                        mediavid.id, mediavid.url_source, 
                        pays.id, pays.sigle, pays.nom
                    ;
                ",
                (Recette, Video, Pays, Images, Tags, Ingredients, Ustensiles) =>
                {
                    Recette.Video = Video;
                    Recette.Pays = Pays;
                    Recette.Images = Images[0].IsNullOrEmpty() ? null : Images.Select(x => x.Split(','))
                                        .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                        .Select(p => new Image { ID = p.Key, URL_Source = p.Value });
                    Recette.Tags = Tags[0].IsNullOrEmpty() ? null : Tags.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1])
                                            .Select(p => new Tag { ID = p.Key, Nom = p.Value });
                    Recette.Ingredients = Ingredients[0].IsNullOrEmpty() ? null : Ingredients.Select(x => x.Split(','))
                                            .ToDictionary(x => int.Parse(x[0]), x => x[1]);
                    Recette.Ustensiles = Ustensiles;
                    return Recette;
                },
                splitOn: "id,id,id,images,tags,ingredients,usetensiles"
            );
        }

        public async Task<int> InsertAsync(Recette model)
        {
            ImageRepository imageRepository = new ImageRepository(connection);
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                int? video = model.Video?.ID;
                int id = await connection.QueryFirstAsync<int>(
                    $@"INSERT INTO {TABLE_NAME} 
                        (nom, temps_preparation, temps_cuisson, temps_repos, date_creation, etape, Fk_Utilisateur) VALUES 
                    (@Nom, @T_prepa, @T_cuisson, @T_repos, @Date_creation, @Etape, @Utilisateur) RETURNING id",
                    new
                    {
                        Nom = model.Nom,
                        T_prepa = model.Temps_Preparation,
                        T_cuisson = model.Temps_Cuisson,
                        T_repos = model.Temps_Repos,
                        Date_creation = model.Date_Creation,
                        Etape = model.Etape,
                        Utilisateur = model.Fk_Utilisateur,
                        Fk_Video = video
                    },
                    transaction);

                if (model.Tags != null)
                {
                    var publicationTagRepository = new ObjectTagRepository(connection, "recette_tag", "fk_recette");
                    await publicationTagRepository.InsertAsync(new ObjectTag { ID = id, Tags = model.Tags }, transaction);
                }
                if (model.Images != null)
                {
                    var objectImageRepository = new ObjectImageRepository(connection, "recette_image", "fk_recette");
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


        public async Task<bool> UpdateAsync(int key, Recette model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            ImageRepository imageRepository = new ImageRepository(connection);
            TagRepository tagRepository = new TagRepository(connection);
            ObjectTagRepository publicationTagRepository = new ObjectTagRepository(connection, "recette_tag", "fk_recette");
            var objectImageRepository = new ObjectImageRepository(connection, "recette_image", "fk_recette");
            IDbTransaction transaction = connection.BeginTransaction();
            try
            {
                int? video = model.Video?.ID;
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
                    },
                    transaction
                );


                if (!await publicationTagRepository.UpdateAsync(key, new ObjectTag { ID = key, Tags = model.Tags }, transaction))
                {
                    throw new System.Exception("Error while updating publication tags");
                }

                if (!await objectImageRepository.UpdateAsync(key, new ObjectImage { ID = key, Images = model.Images }, transaction))
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
