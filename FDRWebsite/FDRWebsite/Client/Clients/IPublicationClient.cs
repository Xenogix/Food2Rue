using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;

namespace FDRWebsite.Client.Clients
{
    public interface IPublicationClient : ICRUDApiClient<Publication, int, PublicationParameters>
    {
    }
}
