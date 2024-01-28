using FDRWebsite.Client.Abstractions.Clients;
using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Clients
{
    public interface IPaysClient : IReadonlyApiClient<Pays, string, EmptyFilterParameters>
    {
    }
}
