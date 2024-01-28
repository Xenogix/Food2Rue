using FDRWebsite.Server.Files;
using Microsoft.AspNetCore.Mvc;

namespace FDRWebsite.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly FileStorageService fileStorage;

        public FilesController(FileStorageService fileStorage)
        {
            this.fileStorage = fileStorage;
        }

        [HttpPost]
        public async Task<IEnumerable<string?>> PostAsync([FromForm] IEnumerable<IFormFile> files)
        {
            var result = new List<string?>();

            foreach (var file in files)
            {
                if (file == null)
                {
                    result.Add(null);
                }
                else
                {
                    using var stream = file.OpenReadStream();
                    result.Add(await fileStorage.StoreAsync(file.FileName, file.ContentType, stream));
                }
            }

            return result;
        }
    }
}
