using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;

namespace FDRWebsite.Client.Clients
{
    public interface IUtilisateurClient : ICRUDApiClient<Utilisateur, int, UtilisateurParameters>
    {
    }
}
