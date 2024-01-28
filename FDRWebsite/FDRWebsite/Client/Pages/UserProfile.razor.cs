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

        [Inject]
        public required IAimeClient AimeClient { get; set; }

        [Parameter]
        public int BufferSize { get; set; } = 10;

        [Parameter]
        public required int UserId { get; set; }

        private IEnumerable<Publication> UserPosts = Array.Empty<Publication>();

        private Utilisateur? user { get; set; }

        private int likeCount { get; set; }

        private int postCount { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            user = await UtilisateurClient.GetAsync(UserId);

            if (user != null)
            {
                await RefreshPostsAsync(user.ID);
                await RefreshLikeCountAsync(user.ID);
            }

            await base.OnParametersSetAsync();
        }

        private async Task LoadPostsAsync(int userID)
        {
            var filter = new PublicationParameters() { UserIDs = new int[] { userID } };
            UserPosts = await PublicationClient.GetFilteredAsync(filter);
            postCount = UserPosts.Count();
        }

        private async Task RefreshPostsAsync(int userID)
        {
            ClearPosts();
            await LoadPostsAsync(userID);
        }

        private void ClearPosts()
        {
            UserPosts = Array.Empty<Publication>();
            postCount = 0;
        }

        private async Task LoadLikeCount()
        {
            var filter = new AimeParameters() { PublicationIDs = UserPosts.Select(p => p.ID) };
            likeCount = (await AimeClient.GetFilteredAsync(filter)).Count();
        }

        private async Task RefreshLikeCountAsync(int userID)
        {
            ClearPosts();
            await LoadPostsAsync(userID);
        }

        private void ClearLikeCount()
        {
            likeCount = 0;
        }
    }
}