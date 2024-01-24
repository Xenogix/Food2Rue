using Dapper;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Npgsql;
using System.Data;

namespace FDRWebsite.Server.Repositories
{
    public class NoteRecetteRepository : IRepositoryBase<NoteRecette, int>
    {
        private const string TABLE_NAME = "note";

        private readonly NpgsqlConnection connection;

        public NoteRecetteRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_utilisateur = @Utilisateur",
                new
                {
                    Utilisateur = key
                }
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int Id_Recette, int Id_Utilisateur, IDbTransaction transaction)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_utilisateur = @Utilisateur AND fk_recette = @Recette",
                new
                {
                    Utilisateur = Id_Utilisateur,
                    Recette = Id_Recette
                },
                transaction
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int Id_Utilisateur, int Id_Recette)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_utilisateur = @Utilisateur AND fk_recette = @Recette",
                new
                {
                    Utilisateur = Id_Utilisateur,
                    Recette = Id_Recette
                }
            );

            return affectedRows > 0;
        }


        public async Task<IEnumerable<NoteRecette>> GetAsync()
        {
            return await connection.QueryAsync<NoteRecette>(
                @$"SELECT fk_utilisateur, fk_recette, note FROM {TABLE_NAME};"
            );
        }

        public async Task<NoteRecette?> GetAsync(int key)
        {
            IEnumerable<NoteRecette> NoteRecette =  await connection.QueryAsync<NoteRecette>(
                @$"SELECT fk_utilisateur, fk_recette, note FROM {TABLE_NAME}
                WHERE fk_utilisateur = @ID;",
                new { ID = key }
            );
            return NoteRecette.FirstOrDefault();
        }

        public async Task<IEnumerable<NoteRecette>> GetAsync(IFilter<NoteRecette> modelFilter)
        {
            return await connection.QueryAsync<NoteRecette>(
                @$"SELECT fk_utilisateur, fk_recette, note FROM {TABLE_NAME}
                WHERE {modelFilter.GetFilterSQL()};"
            );
        }

        public async Task<int> InsertAsync(NoteRecette model, IDbTransaction transaction)
        {
            return await connection.ExecuteAsync(
                @$"INSERT INTO {TABLE_NAME} (fk_utilisateur, fk_recette, note) VALUES 
                (@User, @Recette, @Note);",
                new
                {
                    User = model.Utilisateur,
                    Recette = model.Recette,
                    Note = model.Note
                },
                transaction
            );
        }
        public async Task<int> InsertAsync(NoteRecette model)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            int affectedRows = await InsertAsync(model, transaction);
            if(affectedRows == 0)
            {
                transaction.Rollback();
                return affectedRows;
            }
            transaction.Commit();
            return affectedRows;    
        }
        public async Task<bool> UpdateAsync(int key, NoteRecette model, IDbTransaction transaction)
        {
            if(key != model.Utilisateur)
            {
                return false;
            }
            int row = await connection.ExecuteAsync(
                @$"UPDATE {TABLE_NAME} SET 
                    note = Note
                    WHERE fk_utilisateur = @User AND fk_recette = @Recette;",
                new
                {
                    User = model.Utilisateur,
                    Recette = model.Recette,
                    Note = model.Note
                },
                transaction
            );
            return row> 0;
        }
        public async Task<bool> UpdateAsync(int key, NoteRecette model)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            bool b = await UpdateAsync(key, model, transaction);
            if (!b)
            {
                transaction.Rollback();
                return b;
            }
            transaction.Commit();
            return b;
        }
    }
}
