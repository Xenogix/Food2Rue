using Blazored.LocalStorage;
using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FDRWebsite.Client.Authentication
{
    public class AuthenticationService
    {
        private readonly IAuthenticationClient authenticationClient;

        private readonly ILocalStorageService localStorage;

        private const string STORAGE_KEY = "authToken";

        public AuthenticationService(IAuthenticationClient authenticationClient, ILocalStorageService localStorage)
        {
            this.authenticationClient = authenticationClient;
            this.localStorage = localStorage;

        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var request = new LoginRequest() { Email = email, Password = password };
            var response = await authenticationClient.AuthenticateAsync(request);

            if (response.Token == null) return false;

            await StoreTokenAsync(response.Token);
            return true;
        }

        public async Task LogoutAsync()
        {
            await ClearTokenAsync();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await localStorage.GetItemAsync<string>(STORAGE_KEY);
        }

        private async Task StoreTokenAsync(string token)
        {
            await localStorage.SetItemAsync(STORAGE_KEY, token);
        }

        private async Task ClearTokenAsync()
        {
            await localStorage.RemoveItemAsync(STORAGE_KEY);
        }
    }
}
