using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Models.Filters;
using FDRWebsite.Shared.Models.Objects;

namespace FDRWebsite.Client.Clients
{
    public interface IAimeClient : ICRUDApiClient<Aime, AimeKey, AimeParameters>
    {
    }
}
