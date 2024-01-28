using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models.Objects;
using Npgsql;
using System.Reflection;

namespace FDRWebsite.Server.Repositories
{
    public class AimeRepository : IRepositoryBase<Aime, AimeKey>
    {
        private const string TABLE_NAME = "aime_publication_utilisateur";

        private const string FIELD_NAME = @"fk_publication, fk_utilisateur";

        private const string SELECT_QUERY = @$"SELECT {FIELD_NAME}
                                               FROM {TABLE_NAME}";

        private readonly NpgsqlConnection connection;

        public AimeRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(AimeKey key)
        {
            var affectedRows = await connection.ExecuteAsync($"DELETE FROM {TABLE_NAME} WHERE fk_publication = @fk_publication AND fk_utilisateur = @fk_utilisateur;",
            new
            {
                fk_publication = key.IdPublication,
                fk_utilisateur = key.IdUtilisateur
            });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Aime>> GetAsync()
        {
            return await connection.QueryAsync<Aime>(SELECT_QUERY);
        }

        public async Task<Aime?> GetAsync(AimeKey key)
        {
            var result = await connection.QueryAsync<Aime>(@$"{SELECT_QUERY} WHERE fk_publication = @fk_publication AND fk_utilisateur = @fk_utilisateur;",
            new
            {
                fk_publication = key.IdPublication,
                fk_utilisateur = key.IdUtilisateur
            });

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Aime>> GetAsync(IFilter filter)
        {
            var a = $@"{SELECT_QUERY} WHERE {filter.GetFilterSQL()};";

            return await connection.QueryAsync<Aime>(
               $@"{SELECT_QUERY} WHERE {filter.GetFilterSQL()};",
               filter.GetFilterParameters());
        }

        public async Task<AimeKey> InsertAsync(Aime model)
        {
            return await connection.QueryFirstAsync<AimeKey>(
                @$"INSERT INTO {TABLE_NAME} (fk_publication, fk_utilisateur) VALUES (@FK_Publication, @FK_Utilisateur) RETURNING fk_publication as IdPublication, fk_utilisateur as IdUtilisateur",
                new
                {
                    fk_publication = model.fk_publication,
                    fk_utilisateur = model.fk_utilisateur
                });
        }

        public async Task<bool> UpdateAsync(AimeKey key, Aime model)
        {
            var row = await connection.ExecuteAsync(
                @$"UPDATE {TABLE_NAME} SET 
                        fk_utilisateur = @newIDUtilisateur, 
                        fk_publication = @newIDPublication,
                        WHERE publication.id = @oldIDUtilisateur AND utilisateur.id = @oldIDPublication",
                new
                {
                    newIDUtilisateur = model.fk_utilisateur,
                    newIDPublication = model.fk_publication,
                    oldIDUtilisateur = model.fk_utilisateur,
                    oldIDPublication = model.fk_publication,
                });

            return row > 0;
        }

        public Task<bool> UpdateAsync(int key, Aime model)
        {
            throw new NotImplementedException();
        }
    }
}
