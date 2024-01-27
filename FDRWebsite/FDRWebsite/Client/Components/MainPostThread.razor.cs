using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Components
{
    public partial class MainPostThread
    {
        [Inject]
        public required IPublicationClient PublicationClient { get; set; }

        [Parameter]
        public int BufferSize { get; set; } = 10;

        private IEnumerable<Publication> PostsToLoad = Array.Empty<Publication>();

        protected override async Task OnInitializedAsync()
        {
            await RefreshPostsAsync();
            await base.OnInitializedAsync();
        }

        private async Task LoadPostsAsync()
        {
            PostsToLoad = await PublicationClient.GetAsync();
        }

        private async Task RefreshPostsAsync()
        {
            ClearPosts();
            await LoadPostsAsync();
        }

        private void ClearPosts()
        {
            PostsToLoad = Array.Empty<Publication>();
        }
    }
}