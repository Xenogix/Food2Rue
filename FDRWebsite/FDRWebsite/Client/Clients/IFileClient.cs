using Microsoft.AspNetCore.Http;
using Refit;

namespace FDRWebsite.Client.Clients
{
    public interface IFileClient
    {
        [Multipart]
        [Post("/")]
        Task<IEnumerable<string?>> UploadAsync([AliasAs("files")] IEnumerable<StreamPart> files);
    }
}
