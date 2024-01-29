using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Clients
{
    public interface IRecetteClient : ICRUDApiClient<Recette, int, EmptyFilterParameters>
    {
    }
}
