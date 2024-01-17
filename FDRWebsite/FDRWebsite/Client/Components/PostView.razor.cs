using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Components
{
    public partial class PostView
    {
        [Inject]
        public required IUtilisateurClient utilisateurClient { get; set; }

        [Parameter]
        public required Publication Post { get; set; }

        private Utilisateur? user;

        protected override async Task OnInitializedAsync()
        {
            user = await utilisateurClient.GetAsync(Post.FK_Utilisateur);
            await base.OnInitializedAsync();
        }

        private void OnLikeClick()
        {

        }

        private void OnCommentClick()
        {

        }
    }
}