using FDRWebsite.Shared.Models;
using Refit;

namespace FDRWebsite.Client.Clients
{
    public interface IUserClient : ICRUDApiClient<User, int>
    {
    }
}