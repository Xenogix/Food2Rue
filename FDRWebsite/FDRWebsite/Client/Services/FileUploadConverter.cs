using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace FDRWebsite.Client.Services
{
    public class FileUploadConverter
    {
        public static IFormFile ConvertToFormFile(IBrowserFile browserFile)
        {
            using var stream = browserFile.OpenReadStream(1280000);
            var formFile = new FormFile(stream, 0, stream.Length, null, browserFile.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = browserFile.ContentType
            };
            return formFile;
        }
    }
}
