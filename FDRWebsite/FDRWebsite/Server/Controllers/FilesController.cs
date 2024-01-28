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
        public async Task<string?> PostAsync([FromForm] IFormFile file)
        {
            if (file == null) return null;
            using var stream = file.OpenReadStream();
            return await fileStorage.StoreAsync(file.FileName, file.ContentType, stream);
        }
    }
}
