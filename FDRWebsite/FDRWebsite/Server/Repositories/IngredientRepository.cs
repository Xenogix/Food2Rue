using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class IngredientRepository : IRepositoryBase<Ingredient, int>
    {
        private const string TABLE_NAME = "ingredient";

        private const string FIELD_NAMES = @$"ajoutable.id, 
                                              ajoutable.nom as nom_ajoutable,
                                              ajoutable.description, 
                                              ajoutable.date_publication,
                                              ajoutable.Fk_Utilisateur,
                                              ajoutable.Fk_Administrateur,
                                              ajoutable.est_valide,
                                              allergene.nom as nom,
                                              image.id,
                                              media.url_source";

        private const string SELECT_QUERY = @$"SELECT {FIELD_NAMES}
                                               FROM {TABLE_NAME}
                                               INNER JOIN ajoutable ON ajoutable.id = {TABLE_NAME}.id
                                               LEFT JOIN media ON media.id = {TABLE_NAME}.id
                                               LEFT JOIN image ON image.id = ajoutable.fk_image
                                               LEFT JOIN ingredient_allergene ON ingredient_allergene.fk_ingredient = {TABLE_NAME}.id
                                               LEFT JOIN allergene ON allergene.nom = ingredient_allergene.fk_allergene";

        private const string SPLIT_ON = "nom,id";

        private readonly NpgsqlConnection connection;

        public IngredientRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        private Func<Ingredient, Allergene, Image, Ingredient> GetFieldMapper(Dictionary<int, Ingredient> resultsDictionnary)
        {
            return (ingredient, allergene, image) =>
            {
                if (resultsDictionnary.TryGetValue(ingredient.ID, out var existingIngredient))
                    ingredient = existingIngredient;
                else
                    resultsDictionnary.Add(ingredient.ID, ingredient);

                ingredient.Image = image;

                if(allergene.ID != null)
                    ingredient.Allergenes.Add(allergene);

                return ingredient;
            };
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE id = @ID",
                new { ID = key }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Ingredient>> GetAsync()
        {
            Dictionary<int, Ingredient> results = new();
            await connection.QueryAsync(
                SELECT_QUERY,
                GetFieldMapper(results),
                splitOn: SPLIT_ON
            );
            return results.Values;
        }

        public async Task<Ingredient?> GetAsync(int key)
        {
            Dictionary<int, Ingredient> results = new();
            await connection.QueryAsync(
                $"{SELECT_QUERY} LIMIT 1",
                GetFieldMapper(results),
                splitOn: SPLIT_ON
            );
            return results.Values.FirstOrDefault();
        }

        public async Task<IEnumerable<Ingredient>> GetAsync(IFilter<Ingredient> modelFilter)
        {
            Dictionary<int, Ingredient> results = new();
            await connection.QueryAsync(
                $@"{SELECT_QUERY} WHERE {modelFilter.GetFilterSQL};",
                GetFieldMapper(results),
                splitOn: SPLIT_ON);
            return results.Values;
        }

        public async Task<int> InsertAsync(Ingredient model)
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

        public async Task<bool> UpdateAsync(int key, Ingredient model)
        {
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
