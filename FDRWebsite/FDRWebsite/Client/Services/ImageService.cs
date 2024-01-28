using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using Refit;

namespace FDRWebsite.Client.Services
{
    public class ImageService
    {
        private const int MAX_IMAGE_SIZE = 10000000;

        private IImageClient imageClient { get; set; }

        private IFileClient fileClient { get; set; }


        public ImageService(IImageClient imageClient, IFileClient fileClient)
        {
            this.imageClient = imageClient;
            this.fileClient = fileClient;
        }

        public async Task<Image?> UploadImageAsync(IBrowserFile image)
        {
            var images = await UploadImagesAsync(new IBrowserFile[] { image });
            return images.FirstOrDefault();
        }

        public async Task<IEnumerable<Image?>> UploadImagesAsync(IEnumerable<IBrowserFile> images)
        {
            if (!images.Any()) return Array.Empty<Image?>();
            var streamParts = images.Select(i => new StreamPart(i.OpenReadStream(MAX_IMAGE_SIZE), i.Name, i.ContentType));

            var fileURLs = await fileClient.UploadAsync(streamParts);

            var results = new List<Image?>();
            foreach(var file in fileURLs)
            {
                if (file == null)
                {
                    results.Add(null);
                    continue;
                }

                var imageToInsert = new Image() { URL_Source = file };
                var insertedImageID = await imageClient.InsertAsync(imageToInsert);
                imageToInsert.ID = insertedImageID;
                results.Add(imageToInsert);
            }

            return results;
        }
    }
}
