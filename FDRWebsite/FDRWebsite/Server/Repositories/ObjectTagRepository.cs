using Azure;
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
using System.Data.Common;
using System.Reflection;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace FDRWebsite.Server.Repositories
{
    public class ObjectTagRepository : IRepositoryBase<ObjectTag, int>
    {
        private string TABLE_NAME = "publication_tag";
        private string FK = "fk_publication";

        private readonly NpgsqlConnection connection;

        public ObjectTagRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        public ObjectTagRepository(NpgsqlConnection connection, string table, string fk)
        {
            TABLE_NAME = table;
            FK = fk;
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE {FK} = @Id",
                new
                {
                    Id = key
                }
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int key, int keyTag, IDbTransaction transaction)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE {FK} = @Id AND fk_tag = @IdTag",
                new
                {
                    Id = key,
                    IdTag = keyTag
                },
                transaction
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int key, int keyTag)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            if(!await DeleteAsync(key, keyTag, transaction))
            {
                transaction.Rollback();
                return false;
            }
            
            transaction.Commit();
            return true;
        }

        public async Task<IEnumerable<ObjectTag>> GetAsync()
        {
            IEnumerable<ObjectTag> ObjectTag = await connection.QueryAsync<ObjectTag, string[], ObjectTag>(
                $@"SELECT {TABLE_NAME}.{FK}, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM {TABLE_NAME}
                INNER JOIN tag ON tag.id = {TABLE_NAME}.fk_tag
                GROUP BY {TABLE_NAME}.{FK};",
                (ObjectTag, tags) =>
                {
                    ObjectTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return ObjectTag;
                },
                splitOn: "tags");
            return ObjectTag;
        }

        public async Task<ObjectTag?> GetAsync(int key)
        {
            IEnumerable<ObjectTag> ObjectTag = await connection.QueryAsync<ObjectTag, string[], ObjectTag>(
                $@"SELECT {TABLE_NAME}.{FK}, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM {TABLE_NAME}
                INNER JOIN tag ON tag.id = {TABLE_NAME}.fk_tag
                WHERE {TABLE_NAME}.{FK} = {key}
                GROUP BY {TABLE_NAME}.{FK};",
                (ObjectTag, tags) =>
                {
                    ObjectTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return ObjectTag;
                },
                splitOn: "tags");
            return ObjectTag.FirstOrDefault();
        }

        public async Task<IEnumerable<ObjectTag>> GetAsync(IFilter filter)
        {
            IEnumerable<ObjectTag> ObjectTag = await connection.QueryAsync<ObjectTag, string[], ObjectTag>(
                $@"SELECT {TABLE_NAME}.{FK}, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM {TABLE_NAME}
                INNER JOIN tag ON tag.id = {TABLE_NAME}.fk_tag
                WHERE {filter.GetFilterSQL()}
                GROUP BY {TABLE_NAME}.{FK};",
                (ObjectTag, tags) =>
                {
                    ObjectTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return ObjectTag;
                },
                splitOn: "tags");
            return ObjectTag;
        }


        public async Task<int> InsertAsync(ObjectTag model)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            int row = await InsertAsync(model, transaction);
            if (row == 0)
            {
                transaction.Rollback();
                return row;
            }

            transaction.Commit();
            return row;
        }
        public async Task<int> InsertAsync(ObjectTag model, IDbTransaction transaction)
        {
            TagRepository tagRepository = new TagRepository(connection);
            IEnumerable<Tag> AllTags = await tagRepository.GetAsync();
            IEnumerable<Tag>? Tags = (await GetAsync(key: model.ID))?.Tags;


            int row = 0;

            List<string> list = new List<string> { };
            foreach (var item in model.Tags.Where(Tag => Tags!=null?!Tags.Contains(Tag):true))
            {
                if (AllTags.Contains(item))
                {
                    list.Add($"({model.ID}, {item.ID})");
                }
                else
                {
                    int idTag = await tagRepository.InsertAsync(item);
                    list.Add($"({model.ID}, {idTag}),");

                }
            }
            if (!list.IsNullOrEmpty()) {
                string query = $"INSERT INTO {TABLE_NAME} ({FK}, fk_tag) VALUES {string.Join(',', list)};";
                row += await connection.ExecuteAsync(query, transaction);
            }

            return row;
        }

        public async Task<bool> UpdateAsync(int key, ObjectTag model)
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
        public async Task<bool> UpdateAsync(int key, ObjectTag model, IDbTransaction transaction)
        {
            int row = await InsertAsync(model, transaction);

            TagRepository tagRepository = new TagRepository(connection);
            IEnumerable<Tag> AllTags = await tagRepository.GetAsync();
            IEnumerable<Tag>? Tags = (await GetAsync(key: model.ID))?.Tags;

            if (!Tags.IsNullOrEmpty())
                foreach (var item in Tags.Where(Tag => !model.Tags.Contains(Tag)))
                {
                    await DeleteAsync(model.ID, item.ID, transaction);
                }
            return true;
        }
    }
}
