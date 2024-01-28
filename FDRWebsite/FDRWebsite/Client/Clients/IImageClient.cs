using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Clients
{
    public interface IImageClient : ICRUDApiClient<Image, int, EmptyFilterParameters>
    {
    }
}
