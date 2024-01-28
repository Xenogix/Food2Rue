using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Services
{
    public class FileHelpers
    {
        private const int MAX_THUMBNAIL_SIZE = 1024;

        public static async Task<string> GetThumbnailFromFileAsync(IBrowserFile browserFile)
        {
            var resizedFile = await browserFile.RequestImageFileAsync(browserFile.ContentType, MAX_THUMBNAIL_SIZE, MAX_THUMBNAIL_SIZE);
            var imageBuffer = new byte[resizedFile.Size];
            using var stream = resizedFile.OpenReadStream();
            await stream.ReadAsync(imageBuffer);
            return "data:image/png;base64," + Convert.ToBase64String(imageBuffer);
        }
    }
}
