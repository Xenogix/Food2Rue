using FDRWebsite.Shared.Helpers;
using Microsoft.Extensions.FileProviders;

namespace FDRWebsite.Server.Files
{
    public class FileStorageService
    {
        private readonly string mediaFolderPath;

        public FileStorageService(IConfiguration configuration)
        {
            mediaFolderPath = configuration["MediaFolder"]!;
            if(mediaFolderPath == null) throw new ArgumentNullException(nameof(mediaFolderPath));
        }

        public async Task<string?> StoreAsync(string fileName, string contentType, Stream stream)
        {
            if (!FileFormats.IsSupportedMedia(fileName)) return null;
            var filePath = Path.Combine(mediaFolderPath, Guid.NewGuid().ToString() + Path.GetExtension(fileName));
            Directory.CreateDirectory(mediaFolderPath);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            return filePath;
        }
    }
}
