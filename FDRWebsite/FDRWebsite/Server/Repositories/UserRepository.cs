using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class UserRepository : IRepositoryBase<User, int>
    {
        private const string TABLE_NAME = "utilisateur";

        private readonly NpgsqlConnection connection;

        public UserRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await connection.QueryAsync<User, Image, Pays, User>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.prénom, {TABLE_NAME}.email, {TABLE_NAME}.pseudo, {TABLE_NAME}.password, {TABLE_NAME}.date_naissance, {TABLE_NAME}.date_creation_profil, {TABLE_NAME}.description, media.id, media.url_source, pays.id, pays.sigle, pays.nom FROM {TABLE_NAME} 
                LEFT JOIN media ON {TABLE_NAME}.fk_photo_profil = media.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                ;",
                (User, Image, Pays) =>
                {
                    User.Profile_photo = Image;
                    User.Country = Pays;
                    return User;
                },
                splitOn: "id,id");
        }

        public async Task<User?> GetAsync(int key)
        {
            IEnumerable<User> temps = (IEnumerable<User>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<User>> GetAsync(IFilter<User> modelFilter)
        {
            return await connection.QueryAsync<User, Image, Pays, User>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.prénom, {TABLE_NAME}.email, {TABLE_NAME}.pseudo, {TABLE_NAME}.password, {TABLE_NAME}.date_naissance, {TABLE_NAME}.date_creation_profil, {TABLE_NAME}.description, media.id, media.url_source, pays.id, pays.sigle, pays.nom FROM {TABLE_NAME} 
                LEFT JOIN media ON {TABLE_NAME}.fk_photo_profil = media.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id 
                WHERE {modelFilter.GetFilterSQL()}
                ;",
                (User, Image, Pays) =>
                {
                    User.Profile_photo = Image;
                    User.Country = Pays;
                    return User;
                },
                splitOn: "id,id");
        }

        public async Task<int> InsertAsync(User model)
        {
            return await connection.QueryFirstAsync<int>(
                @$"INSERT INTO {TABLE_NAME}(nom, prenom, email, fk_photo_profil, pseudo, password, date_naissance, date_creation_profil, description, fk_pays) VALUES 
                (@FirstName, @LastName, @Email, @Profile_photo, @Pseudo, @Password, @ProfileCreation, @Description, @Country) RETURNING id",
                new
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Profile_photo = model.Profile_photo.ID,
                    Pseudo = model.Pseudo,
                    Password = model.Password,
                    BirthDate = model.BirthDate,
                    ProfileCreation = model.Profile_Creation,
                    Description = model.Description,
                    Country = model.Country.ID
                });
        }

        public async Task<bool> UpdateAsync(int key, User model)
        {
            var row =  await connection.ExecuteAsync(
                $"UPDATE {TABLE_NAME} SET " +
                        $"nom = @FirstName, " +
                        $"prenom = @LastName, " +
                        $"email = @Email, " +
                        $"fk_photo_profil = @Profile_photo, " +
                        $"pseudo = @Pseudo, " +
                        $"password = @Password, " +
                        $"date_naissance = @BirthDate, " +
                        $"date_creation_profil = @ProfileCreation, " +
                        $"description = @Description, " +
                        $"fk_pays = @Country " +
                        $"WHERE id = @Id;",
            new
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Profile_photo = model.Profile_photo.ID,
                Pseudo = model.Pseudo,
                Password = model.Password,
                BirthDate = model.BirthDate,
                ProfileCreation = model.Profile_Creation,
                Description = model.Description,
                Country = model.Country.ID,
                Id = key
            });

            return row > 0;
        }
    }
}
