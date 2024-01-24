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

namespace FDRWebsite.Server.Repositories
{
    public class PublicationImageRepository : IRepositoryBase<PublicationImage, int>
    {
        private const string TABLE_NAME = "publication_image";

        private readonly NpgsqlConnection connection;

        public PublicationImageRepository(NpgsqlConnection connection)
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
        public async Task<bool> DeleteAsync(int keyPublication, int keyImage, IDbTransaction transaction)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE fk_publication = @IdPup AND fk_image = @IdImage",
                new
                {
                    IdPup = keyPublication,
                    IdImage = keyImage
                },
                transaction
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int keyPublication, int keyTag)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            if (!await DeleteAsync(keyPublication, keyTag, transaction))
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }

        public async Task<IEnumerable<PublicationImage>> GetAsync()
        {
            IEnumerable<PublicationImage> PublicationImages = await connection.QueryAsync<PublicationImage, string[], PublicationImage>(
                $@"SELECT publication_image.fk_publication, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM publication_image
                INNER JOIN media ON media.id = publication_image.fk_image
                INNER JOIN image ON image.id = media.id
                GROUP BY publication_image.fk_publication;",
                (PublicationImage, images) =>
                {
                    PublicationImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return PublicationImage;
                },
                splitOn: "images");
            return PublicationImages;
        }

        public async Task<PublicationImage?> GetAsync(int key)
        {
            IEnumerable<PublicationImage> PublicationImages = await connection.QueryAsync<PublicationImage, string[], PublicationImage>(
                $@"SELECT publication_image.fk_publication, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM publication_image
                INNER JOIN media ON media.id = publication_image.fk_image
                INNER JOIN image ON image.id = media.id
                WHERE publication_image.fk_publication = {key}
                GROUP BY publication_image.fk_publication;",
                (PublicationImage, images) =>
                {
                    PublicationImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return PublicationImage;
                },
                splitOn: "images");
            return PublicationImages.FirstOrDefault();
        }

        public async Task<IEnumerable<PublicationImage>> GetAsync(IFilter<PublicationImage> modelFilter)
        {
            IEnumerable<PublicationImage> PublicationImages = await connection.QueryAsync<PublicationImage, string[], PublicationImage>(
                $@"SELECT publication_image.fk_publication, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM publication_image
                INNER JOIN media ON media.id = publication_image.fk_image
                INNER JOIN image ON image.id = media.id
                WHERE {modelFilter.GetFilterSQL()}
                GROUP BY publication_image.fk_publication;",
                (PublicationImage, images) =>
                {
                    PublicationImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return PublicationImage;
                },
                splitOn: "images");
            return PublicationImages;
        }

        public async Task<int> InsertAsync(PublicationImage model)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            int row = await InsertAsync(model, transaction);
            if (row==0)
            {
                transaction.Rollback();
                return row;
            }

            transaction.Commit();
            return row;
        }
        public async Task<int> InsertAsync(PublicationImage model, IDbTransaction transaction)
        {
            ImageRepository ImageRepository = new ImageRepository(connection);
            IEnumerable<Image> AllImage = await ImageRepository.GetAsync();
            IEnumerable<Image>? Images = (await GetAsync(key: model.ID))?.Images;

            int row = 0;

            List<string> list = new List<string> { };
            foreach (var item in model.Images.Where(Image => Images != null ? !Images.Contains(Image) : true))
            {
                if (AllImage.Contains(item))
                {
                    list.Add($"({model.ID}, {item.ID})");
                }
                else
                {
                    int idTag = await ImageRepository.InsertAsync(item);
                    list.Add($"({model.ID}, {idTag}),");

                }
            }
            if (!list.IsNullOrEmpty())
            {
                string query = $"INSERT INTO publication_image (fk_publication, fk_image) VALUES {String.Join(',', list)};";
                row += await connection.ExecuteAsync(query, transaction);
            }

            return row;
        }


        public async Task<bool> UpdateAsync(int key, PublicationImage model)
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

        public async Task<bool> UpdateAsync(int key, PublicationImage model, IDbTransaction transaction)
        {
            int row = await InsertAsync(model, transaction);

            ImageRepository ImageRepository = new ImageRepository(connection);
            IEnumerable<Image> AllImage = await ImageRepository.GetAsync();
            IEnumerable<Image>? Images = (await GetAsync(key: model.ID))?.Images;

            if (!Images.IsNullOrEmpty())
                foreach (var item in Images.Where(Image => model.Images != null ? !model.Images.Contains(Image) : true))
                {
                    await DeleteAsync(model.ID, item.ID, transaction);
                }
            return true;
        }
    }
}
