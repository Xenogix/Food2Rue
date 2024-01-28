using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Models.Objects;
using Npgsql;

namespace FDRWebsite.Server.Repositories
{
    public class AimeRepository : IRepositoryBase<Aime, int>
    {
        private const string TABLE_NAME = "aime_publication_utilisateur";

        private const string FIELD_NAME = @"publication.id, utilisateur.id";

        private const string SELECT_QUERY = @$"SELECT {FIELD_NAME}
                                               FROM {TABLE_NAME}";

        private readonly NpgsqlConnection connection;

        public AimeRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE id = @ID",
                new { ID = key }
            );

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Aime>> GetAsync()
        {
            return (IEnumerable<Aime>)await connection.QueryAsync(SELECT_QUERY);
        }

        /// <summary>
        /// NOTE : todo class to prevent bug
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Aime?> GetAsync(Tuple<int, int> key)
        {
            return (Aime?)await connection.QueryAsync(@$"{SELECT_QUERY} WHERE publication.id = {key.Item1} utilisateur.id = {key.Item2};");
        }

        public async Task<IEnumerable<Aime>> GetAsync(IFilter filter)
        {
            return (IEnumerable<Aime>)await connection.QueryAsync(
               $@"{SELECT_QUERY} WHERE {filter.GetFilterSQL};");
        }

        public async Task<int> InsertAsync(Aime model)
        {
            return await connection.QueryFirstAsync<int>(
                @$"INSERT INTO {TABLE_NAME} (fk_publication, fk_utilisateur) VALUES (@FK_Publication, FK_Utilisateur) RETURNING id",
                new
                {
                    fk_publication = model.IdPublication,
                    fk_utilisateur = model.IdUtilisateur
                });
        }

        public async Task<bool> UpdateAsync(Tuple<int, int> key, Aime model)
        {
            var row = await connection.ExecuteAsync(
                @$"UPDATE {TABLE_NAME} SET 
                        fk_utilisateur = @FK_Utilisateur, 
                        fk_publication = @FK_Publication,
                        WHERE publication.id = {key.Item1} AND utilisateur.id = {key.Item2}",
                new
                {
                    FK_Utilisateur = model.IdUtilisateur,
                    FK_Publication = model.IdPublication,
                    //todo
                });

            return row > 0;
        }
    }
}
