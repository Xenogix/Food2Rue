using Microsoft.AspNetCore.Http;
using Refit;

namespace FDRWebsite.Client.Clients
{
    public interface IFileClient
    {
        [Multipart]
        [Post("/")]
        Task<string?> UploadAsync([AliasAs("file")] StreamPart file);
    }
}
