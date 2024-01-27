using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class UtilisateurRepository : IRepositoryBase<Utilisateur, int>
    {
        private const string TABLE_NAME = "utilisateur";

        private readonly NpgsqlConnection connection;

        public UtilisateurRepository(NpgsqlConnection connection)
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

        public async Task<IEnumerable<Utilisateur>> GetAsync()
        {
            return await connection.QueryAsync<Utilisateur, Image, Pays, Utilisateur>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.prénom, {TABLE_NAME}.email, {TABLE_NAME}.pseudo, {TABLE_NAME}.password, {TABLE_NAME}.description, {TABLE_NAME}.date_naissance, {TABLE_NAME}.date_creation_profil, media.id, media.url_source, pays.id, pays.sigle, pays.nom FROM {TABLE_NAME} 
                LEFT JOIN media ON {TABLE_NAME}.fk_photo_profil = media.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id
                ;",
                (User, Image, Pays) =>
                {
                    User.Photo_Profil = Image;
                    User.Pays = Pays;
                    return User;
                },
                splitOn: "id,id");
        }

        public async Task<Utilisateur?> GetAsync(int key)
        {
            IEnumerable<Utilisateur> temps = (IEnumerable<Utilisateur>)await GetAsync();
            var U = temps.Where(temp => temp.ID == key).ToList();
            if (U.Count == 0)
                return null;
            else
                return U[0];
        }

        public async Task<IEnumerable<Utilisateur>> GetAsync(IFilter filter)
        {
            var a = $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.prénom, {TABLE_NAME}.email, {TABLE_NAME}.pseudo, {TABLE_NAME}.password, {TABLE_NAME}.date_naissance, {TABLE_NAME}.date_creation_profil, {TABLE_NAME}.description, media.id, media.url_source, pays.id, pays.sigle, pays.nom FROM {TABLE_NAME} 
                LEFT JOIN media ON {TABLE_NAME}.fk_photo_profil = media.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id 
                WHERE {filter.GetFilterSQL()};";

            return await connection.QueryAsync<Utilisateur, Image, Pays, Utilisateur>(
                $@"SELECT {TABLE_NAME}.id, {TABLE_NAME}.nom, {TABLE_NAME}.prénom, {TABLE_NAME}.email, {TABLE_NAME}.pseudo, {TABLE_NAME}.password, {TABLE_NAME}.date_naissance, {TABLE_NAME}.date_creation_profil, {TABLE_NAME}.description, media.id, media.url_source, pays.id, pays.sigle, pays.nom FROM {TABLE_NAME} 
                LEFT JOIN media ON {TABLE_NAME}.fk_photo_profil = media.id
                LEFT JOIN pays ON {TABLE_NAME}.fk_pays = pays.id 
                WHERE {filter.GetFilterSQL()};",
                (User, Image, Pays) =>
                {
                    User.Photo_Profil = Image;
                    User.Pays = Pays;
                    return User;
                },
                filter.GetFilterParameters(),
                splitOn: "id,id");
        }

        public async Task<int> InsertAsync(Utilisateur model)
        {
            return await connection.QueryFirstAsync<int>(
                @$"INSERT INTO {TABLE_NAME} (nom, prénom, email, fk_photo_profil, pseudo, password, date_naissance, date_creation_profil, description, fk_pays) VALUES 
                (@FirstName, @LastName, @Email, @Profile_photo, @Pseudo, @Password, @BirthDay, @ProfileCreation, @Description, @Country) RETURNING id",
                new
                {
                    FirstName = model.Prénom,
                    LastName = model.Nom,
                    Email = model.Email,
                    Profile_photo = model.Photo_Profil.ID,
                    Pseudo = model.Pseudo,
                    Password = model.Password,
                    BirthDay = model.Date_Naissance,
                    ProfileCreation = model.Date_Creation_Profil,
                    Description = model.Description,
                    Country = model.Pays.ID
                });
        }

        public async Task<bool> UpdateAsync(int key, Utilisateur model)
        {
            if (!model.ID.Equals(0) && key != model.ID)
            {
                return false;
            }
            var row =  await connection.ExecuteAsync(
                $"UPDATE {TABLE_NAME} SET " +
                        $"nom = @FirstName, " +
                        $"prénom = @LastName, " +
                        $"email = @Email, " +
                        $"fk_photo_profil = @Profile_photo, " +
                        $"pseudo = @Pseudo, " +
                        $"password = @Password, " +
                        $"date_naissance = @BirthDay, " +
                        $"date_creation_profil = @ProfileCreation, " +
                        $"description = @Description, " +
                        $"fk_pays = @Country " +
                        $"WHERE id = @Id;",
            new
            {
                FirstName = model.Prénom,
                LastName = model.Nom,
                Email = model.Email,
                Profile_photo = model.Photo_Profil.ID,
                Pseudo = model.Pseudo,
                Password = model.Password,
                BirthDay = model.Date_Naissance,
                ProfileCreation = model.Date_Creation_Profil,
                Description = model.Description,
                Country = model.Pays.ID,
                Id = key
            });

            return row > 0;
        }
    }
}
