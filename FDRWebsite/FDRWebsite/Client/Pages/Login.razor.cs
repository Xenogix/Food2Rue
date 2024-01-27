using FDRWebsite.Client.Authentication;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Pages
{
    public partial class Login
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        private string? email;
        private string? password;
        private bool loginHasFailed;

        private async void SignInAsync()
        {
            if (email == null || password == null) return;

            loginHasFailed = !(await AuthenticationService.LoginAsync(email, password));

            if(!loginHasFailed)
                NavigationManager.NavigateTo("/");
            else
                StateHasChanged();
        }
    }
}