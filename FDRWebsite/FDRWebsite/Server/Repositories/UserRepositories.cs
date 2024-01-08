using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Server.Repositories
{
    public class UserRepository : IRepositoryBase<User, int>
    {
        private const string TABLE_NAME = "utilisateur";
        public async Task<bool> DeleteAsync(int key)
        {
            await Task.CompletedTask;
            String query = "DELETE FROM " + TABLE_NAME + " WHERE id = " + key;
            //TO DO : execute query
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            await Task.CompletedTask;
            String query = "SELECT " + TABLE_NAME + ".id, nom, prenom, email, fk_photo_profil, pseudo, password, date_naissance, date_creation_profil, description, fk_pays, publication.id AS Publication, recette.id AS Recette, aime_publication_utilisateur.fk_publication AS Aime FROM " + TABLE_NAME +
                "LEFT JOIN recette ON recette.fk_utilisateur = " + TABLE_NAME + ".id" +
                "LEFT JOIN publication ON publication.fk_utilisateur = " + TABLE_NAME + ".id" +
                "LEFT JOIN aime_publication_utilisateur ON aime_publication_utilisateur.fk_utilisateur = " + TABLE_NAME + ".id" +
                ";";
            //TO DO : execute query
            throw new NotImplementedException();
        }

        public async Task<User?> GetAsync(int key)
        {
            IEnumerable<User> temps = (IEnumerable<User>)GetAsync();
            await Task.CompletedTask;
            return (User?)temps.Where(temp => temp.ID == key);
        }

        public async Task<IEnumerable<User>> GetAsync(IFilter<User> modelFilter)
        {
            // TO DO : Implementation getAsync with model
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(User model)
        {
            await Task.CompletedTask;
            String query = "INSERT INTO " + TABLE_NAME + "(nom, prenom, email, fk_photo_profil, pseudo, password, date_naissance, date_creation_profil, description, fk_pays) VALUES " +
                "(" + model.FirstName + "," +
                model.LastName + "," +
                model.Email + "," +
                model.Profile_photo.ID + "," +
                model.Pseudo + "," +
                model.Password + "," +
                model.BirthDate + "," +
                model.Profile_Creation + "," +
                model.Description + "," +
                model.Country.ID + ") RETURNING id;";

            //TO DO : execute query
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(int key, User model)
        {
            await Task.CompletedTask;
            String query = "UPDATE INTO " + TABLE_NAME + "SET " +
                "nom = " + model.FirstName + "," +
                "prenom = " + model.LastName + "," +
                "email = " + model.Email + "," +
                "fk_photo_profil = " + model.Profile_photo.ID + "," +
                "pseudo = " + model.Pseudo + "," +
                "password = " + model.Password + "," +
                "date_naissance = " + model.BirthDate + "," +
                "date_creation_profil = " + model.Profile_Creation + "," +
                "description = " + model.Description + "," +
                "fk_pays = " + model.Country.ID +
                "WHERE id = " + key + ";";
            throw new NotImplementedException();
        }
    }
}
