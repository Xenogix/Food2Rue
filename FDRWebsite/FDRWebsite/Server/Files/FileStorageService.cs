using FDRWebsite.Shared.Helpers;
using Microsoft.Extensions.FileProviders;

namespace FDRWebsite.Server.Files
{
    public class FileStorageService
    {
        private readonly string mediaFolderPath;

        private const string ROOT_FOLDER = ".\\wwwroot";

        public FileStorageService(IConfiguration configuration)
        {
            mediaFolderPath = configuration["MediaFolder"]!;
            if(mediaFolderPath == null) throw new ArgumentNullException(nameof(mediaFolderPath));
        }

        public async Task<string?> StoreAsync(string fileName, string contentType, Stream stream)
        {
            if (!FileFormats.IsSupportedMedia(fileName)) return null;
            var filePath = Path.Combine(ROOT_FOLDER, mediaFolderPath, Guid.NewGuid().ToString() + Path.GetExtension(fileName));
            Directory.CreateDirectory(mediaFolderPath);
            using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
            return filePath.Substring(ROOT_FOLDER.Length, filePath.Length - ROOT_FOLDER.Length).Replace("\\", "/");
        }
    }
}
