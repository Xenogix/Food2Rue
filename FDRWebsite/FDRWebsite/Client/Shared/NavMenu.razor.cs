using FDRWebsite.Client.Models;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FDRWebsite.Client.Shared
{
    public partial class NavMenu
    {
        [CascadingParameter]
        public required Task<AuthenticationState> AuthStateTask { get; set; }

        private readonly List<NavigationEntry> navigationEntries = new();


        protected async override Task OnParametersSetAsync()
        {
            navigationEntries.Clear();
            navigationEntries.Add(new() { Title = "Home", Url = "/", Icon = "fa-house" });

            var authenticationState = await AuthStateTask;

            if (authenticationState.User.Identity?.IsAuthenticated == true)
            {
                var userID = authenticationState.User.FindFirst(UserClaimTypes.ID)!.Value;

                navigationEntries.Add(new() { Title = "Profile", Url = $"/userprofile/{userID}", Icon = "fa-address-card" });
                navigationEntries.Add(new() { Title = "Post", Url = "/post", Icon = "fa-square-plus" });
                navigationEntries.Add(new() { Title = "Logout", Url = "/logout", Icon = "fa-key" });
            }
            else
            {
                navigationEntries.Add(new() { Title = "Login", Url = "/login", Icon = "fa-key" });
            }
        }
    }
}