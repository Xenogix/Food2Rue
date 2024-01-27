using FDRWebsite.Client.Authentication;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Pages
{
    public partial class Logout
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await AuthenticationService.LogoutAsync();
            NavigationManager.NavigateTo("/");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}