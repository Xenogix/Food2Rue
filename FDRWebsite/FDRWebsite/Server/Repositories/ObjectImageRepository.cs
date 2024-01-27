using Dapper;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Server.Abstractions.Repositories;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Data;

namespace FDRWebsite.Server.Repositories
{
    public class ObjectImageRepository : IRepositoryBase<ObjectImage, int>
    {
        private string TABLE_NAME = "publication_image";
        private string FK = "fk_publication";

        private readonly NpgsqlConnection connection;

        public ObjectImageRepository(NpgsqlConnection connection)
        {
            this.connection = connection;
        }
        public ObjectImageRepository(NpgsqlConnection connection, string table, string fk)
        {
            TABLE_NAME = table;
            FK = fk;
            this.connection = connection;
        }

        public async Task<bool> DeleteAsync(int key)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE @Champ = @Id",
                new
                {
                    Champ = FK,
                    Id = key
                }
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int key1, int keyImage, IDbTransaction transaction)
        {
            var affectedRows = await connection.ExecuteAsync(
                $"DELETE FROM {TABLE_NAME} WHERE @Champ1 = @IdPup AND fk_image = @IdImage",
                new
                {
                    Champ1 = FK,
                    Id1 = key1,
                    IdImage = keyImage
                },
                transaction
            );

            return affectedRows > 0;
        }
        public async Task<bool> DeleteAsync(int key1, int keyImage)
        {
            IDbTransaction transaction = connection.BeginTransaction();
            if (!await DeleteAsync(key1, keyImage, transaction))
            {
                transaction.Rollback();
                return false;
            }

            transaction.Commit();
            return true;
        }

        public async Task<IEnumerable<ObjectImage>> GetAsync()
        {
            IEnumerable<ObjectImage> ObjectImage = await connection.QueryAsync<ObjectImage, string[], ObjectImage>(
                $@"SELECT {TABLE_NAME}.{FK}, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM publication_image
                INNER JOIN media ON media.id = publication_image.fk_image
                INNER JOIN image ON image.id = media.id
                GROUP BY {TABLE_NAME}.{FK};",
                (ObjectImage, images) =>
                {
                    ObjectImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return ObjectImage;
                },
                splitOn: "images");
            return ObjectImage;
        }

        public async Task<ObjectImage?> GetAsync(int key)
        {
            IEnumerable<ObjectImage> ObjectImages = await connection.QueryAsync<ObjectImage, string[], ObjectImage>(
                $@"SELECT {TABLE_NAME}.{FK}, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM publication_image
                INNER JOIN media ON media.id = publication_image.fk_image
                INNER JOIN image ON image.id = media.id
                WHERE {TABLE_NAME}.{FK} = {key}
                GROUP BY {TABLE_NAME}.{FK};",
                (ObjectImage, images) =>
                {
                    ObjectImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return ObjectImage;
                },
                splitOn: "images");
            return ObjectImages.FirstOrDefault();
        }

        public async Task<IEnumerable<ObjectImage>> GetAsync(IFilter filter)
        {
            IEnumerable<ObjectImage> ObjectImages = await connection.QueryAsync<ObjectImage, string[], ObjectImage>(
                $@"SELECT {TABLE_NAME}.{FK}.fk_publication, 
                array_agg(DISTINCT media.id || ',' || media.url_source) AS images 
                FROM {TABLE_NAME}.{FK}
                INNER JOIN media ON media.id = {TABLE_NAME}.{FK}.fk_image
                INNER JOIN image ON image.id = media.id
                WHERE {filter.GetFilterSQL()}
                GROUP BY {TABLE_NAME}.{FK}.fk_publication;",
                (ObjectImage, images) =>
                {
                    ObjectImage.Images = images.Select(image =>
                    {
                        string[] imageSplit = image.Split(',');
                        return new Image
                        {
                            ID = int.Parse(imageSplit[0]),
                            URL_Source = imageSplit[1]
                        };
                    });
                    return ObjectImage;
                },
                splitOn: "images");
            return ObjectImages;
        }

        public async Task<int> InsertAsync(ObjectImage model)
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
        public async Task<int> InsertAsync(ObjectImage model, IDbTransaction transaction)
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
                string query = $"INSERT INTO {TABLE_NAME} ({FK}, fk_image) VALUES {string.Join(',', list)};";
                row += await connection.ExecuteAsync(query, transaction);
            }

            return row;
        }


        public async Task<bool> UpdateAsync(int key, ObjectImage model)
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

        public async Task<bool> UpdateAsync(int key, ObjectImage model, IDbTransaction transaction)
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
