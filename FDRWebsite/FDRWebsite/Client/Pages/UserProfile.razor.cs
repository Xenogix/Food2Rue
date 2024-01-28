using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Pages
{
    public partial class UserProfile
    {
        [Inject]
        public required IUtilisateurClient UtilisateurClient { get; set; }

        [Inject]
        public required IPublicationClient PublicationClient { get; set; }

        [Parameter]
        public int BufferSize { get; set; } = 10;

        [Parameter]
        public required int UserId { get; set; }

        private IEnumerable<Publication> UserPosts = Array.Empty<Publication>();

        private Utilisateur? user { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            user = await UtilisateurClient.GetAsync(UserId);

            if (user != null)
                await RefreshPostsAsync(user.ID);

            await base.OnParametersSetAsync();
        }

        private async Task LoadPostsAsync(int userID)
        {
            var filter = new PublicationParameters() { UserIDs = new int[] { userID } };
            UserPosts = await PublicationClient.GetFilteredAsync(filter);
        }

        private async Task RefreshPostsAsync(int userID)
        {
            ClearPosts();
            await LoadPostsAsync(userID);
        }

        private void ClearPosts()
        {
            UserPosts = Array.Empty<Publication>();
        }
    }
}