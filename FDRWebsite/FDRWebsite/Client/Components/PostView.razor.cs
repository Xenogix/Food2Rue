using FDRWebsite.Client.Clients;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using FDRWebsite.Shared.Models.Objects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FDRWebsite.Client.Components
{
    public partial class PostView
    {
        [Inject]
        public required IUtilisateurClient utilisateurClient { get; set; }

        [Inject]
        public required IAimeClient aimeClient { get; set; }

        [Parameter]
        public required Publication Post { get; set; }

        [CascadingParameter]
        public required Task<AuthenticationState> AuthStateTask { get; set; }

        private Utilisateur? user;

        private int? currentUsedID;

        private bool liked;

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthStateTask;

            if (authenticationState.User.Identity?.IsAuthenticated == true)
            {
                var stringUserId = authenticationState.User.FindFirst(UserClaimTypes.ID)!.Value;
                if (int.TryParse(stringUserId, out int userID))
                    currentUsedID = userID;
            }

            user = await utilisateurClient.GetAsync(Post.FK_Utilisateur);

            if(currentUsedID != null)
            {
                var likes = (await aimeClient.GetFilteredAsync(new AimeParameters() { PublicationIDs = new int[] { Post.ID } }));
                liked = (await aimeClient.GetFilteredAsync(new AimeParameters() { PublicationIDs = new int[] { Post.ID } })).Any(a => a.fk_utilisateur == currentUsedID);
            }

            await base.OnInitializedAsync();
        }

        private async void OnLikeClickAsync()
        {
            if (currentUsedID == null) return;


            if(liked)
            {
                await aimeClient.DeleteAsync(new AimeKey() { IdPublication = Post.ID, IdUtilisateur = currentUsedID.Value });
                Post.Aime -= 1;
            }
            else
            {
                await aimeClient.InsertAsync(new Aime() { fk_publication = Post.ID, fk_utilisateur = currentUsedID.Value });
                Post.Aime += 1;
            }

            liked = !liked;

            StateHasChanged();
        }

        private void OnCommentClick()
        {
            // TODO Implement comment
        }
    }
}