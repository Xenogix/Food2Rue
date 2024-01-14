using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;

namespace FDRWebsite.Server.Repositories
{
    public class UstensileRepository : IRepositoryBase<Ustensile, int>
    {
        private const string TABLE_NAME = "ustensile";

        private readonly NpgsqlConnection connection;

        public UstensileRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Ustensile>> GetAsync()
        {
            return await connection.QueryAsync<Ustensile, Image, Ustensile>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.description, {TABLE_NAME}.date_publication
                {TABLE_NAME}.Fk_Utilisateur
                {TABLE_NAME}.Fk_Administrateur
                {TABLE_NAME}.est_valide
                image.id, image.url_source,
                FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                INNER JOIN image ON image.id = media.id
                ;",
                (Ingredient, Image) =>
                {
                    Ingredient.Image = Image;
                    return Ingredient;
                },
                splitOn: "id,id");
        }

        public async Task<Ustensile?> GetAsync(int key)
        {
            IEnumerable<Ustensile> temps = await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Ustensile>> GetAsync(IFilter<Ustensile> modelFilter)
        {
            return await connection.QueryAsync<Ustensile, Image, Ustensile>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.description, {TABLE_NAME}.date_publication
                {TABLE_NAME}.Fk_Utilisateur
                {TABLE_NAME}.Fk_Administrateur
                {TABLE_NAME}.est_valide
                image.id, image.url_source,
                FROM {TABLE_NAME}
                LEFT JOIN media ON media.id = {TABLE_NAME}.id
                INNER JOIN image ON image.id = media.id
                WHERE {modelFilter.GetFilterSQL}
                ;",
                (Ingredient, Image) =>
                {
                    Ingredient.Image = Image;
                    return Ingredient;
                },
                splitOn: "id,id");
        }

        public async Task<int> InsertAsync(Ustensile model)
        {
            return await connection.QueryFirstAsync<int>(
                $@"INSERT INTO {TABLE_NAME} (nom, description, date_publication, fk_image, fk_utilisateur, fk_administrateur, est_valide) VALUES 
                (@Nom, @Description, @Date_Publication, @Fk_Image, @Fk_Utilisateur, @Fk_Administrateur, @Est_Valide) RETURNING id",
                new
                {
                    Nom = model.Nom,
                    Description = model.Description,
                    Date_Publication = model.Date_Publication,
                    Fk_Image = model.Image.ID,
                    Fk_Utilisateur = model.Fk_Utilisateur,
                    Fk_Administrateur = model.Fk_Administrateur,
                    Est_Valide = model.Est_Valide
                });
        }

        public async Task<bool> UpdateAsync(int key, Ustensile model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row = await connection.ExecuteAsync(
                @$"UPDATE {TABLE_NAME} SET 
                nom = @Nom, 
                description = @Description, 
                date_publication = @Date_Publication, 
                fk_image = @Fk_Image,
                fk_utilisateur = @Fk_Utilisateur, 
                fk_administrateur = @Fk_Administrateur, 
                est_valide = @Est_Valide
                WHERE id = @Id",
                new
                {
                    Nom = model.Nom,
                    Description = model.Description,
                    Date_Publication = model.Date_Publication,
                    Fk_Utilisateur = model.Fk_Utilisateur,
                    Fk_Administrateur = model.Fk_Administrateur,
                    Est_Valide = model.Est_Valide,
                    Id = key
                });
            return row > 0;
        }

    }
}
