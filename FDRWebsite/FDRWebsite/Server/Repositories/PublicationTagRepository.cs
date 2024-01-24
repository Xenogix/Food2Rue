using Azure;
using Dapper;
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
    public class PublicationTagRepository : IRepositoryBase<PublicationTag, int>
    {
        private const string TABLE_NAME = "publication_tag";

        private readonly NpgsqlConnection connection;

        public PublicationTagRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_publication = @Id",
                new
                {
                    Id = key
                }
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int keyPublication, int keyTag, IDbTransaction transaction)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_publication = @IdPup AND fk_tag = @IdTag",
                new
                {
                    IdPup = keyPublication,
                    IdTag = keyTag
                },
                transaction
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int keyPublication, int keyTag)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            if(!await DeleteAsync(keyPublication, keyTag, transaction))
            {
                transaction.Rollback();
                return false;
            }
            
            transaction.Commit();
            return true;
        }

        public async Task<IEnumerable<PublicationTag>> GetAsync()
        {
            IEnumerable<PublicationTag> publicationTags = await connection.QueryAsync<PublicationTag, string[], PublicationTag>(
                $@"SELECT publication_tag.fk_publication, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM publication_tag
                INNER JOIN tag ON tag.id = publication_tag.fk_tag
                GROUP BY publication_tag.fk_publication;",
                (PublicationTag, tags) =>
                {
                    PublicationTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return PublicationTag;
                },
                splitOn: "tags");
            return publicationTags;
        }

        public async Task<PublicationTag?> GetAsync(int key)
        {
            IEnumerable<PublicationTag> publicationTags = await connection.QueryAsync<PublicationTag, string[], PublicationTag>(
                $@"SELECT publication_tag.fk_publication, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM publication_tag
                INNER JOIN tag ON tag.id = publication_tag.fk_tag
                WHERE publication_tag.fk_publication = {key}
                GROUP BY publication_tag.fk_publication;",
                (PublicationTag, tags) =>
                {
                    PublicationTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return PublicationTag;
                },
                splitOn: "tags");
            return publicationTags.FirstOrDefault();
        }

        public async Task<IEnumerable<PublicationTag>> GetAsync(IFilter<PublicationTag> modelFilter)
        {
            IEnumerable<PublicationTag> publicationTags = await connection.QueryAsync<PublicationTag, string[], PublicationTag>(
                $@"SELECT publication_tag.fk_publication, 
                array_agg(DISTINCT tag.id || ',' || tag.nom) AS tags 
                FROM publication_tag
                INNER JOIN tag ON tag.id = publication_tag.fk_tag
                WHERE publication_tag.fk_publication = {modelFilter.GetFilterSQL()}
                GROUP BY publication_tag.fk_publication;",
                (PublicationTag, tags) =>
                {
                    PublicationTag.Tags = tags.Select(tag =>
                    {
                        string[] tagSplit = tag.Split(',');
                        return new Tag
                        {
                            ID = int.Parse(tagSplit[0]),
                            Nom = tagSplit[1]
                        };
                    });
                    return PublicationTag;
                },
                splitOn: "tags");
            return publicationTags;
        }


        public async Task<int> InsertAsync(PublicationTag model)
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
        public async Task<int> InsertAsync(PublicationTag model, IDbTransaction transaction)
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
                string query = $"INSERT INTO publication_tag (fk_publication, fk_tag) VALUES {String.Join(',', list)};";
                row += await connection.ExecuteAsync(query, transaction);
            }

            return row;
        }

        public async Task<bool> UpdateAsync(int key, PublicationTag model)
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
        public async Task<bool> UpdateAsync(int key, PublicationTag model, IDbTransaction transaction)
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
