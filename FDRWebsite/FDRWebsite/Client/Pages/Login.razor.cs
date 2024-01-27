using FDRWebsite.Client.Authentication;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Pages
{
    public partial class Login
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        private string? email;
        private string? password;

        private async void SignInAsync()
        {
            if (email == null || password == null) return;

            await AuthenticationService.LoginAsync(email, password);
        }
    }
}