using FDRWebsite.Client.Authentication;
using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Forms;
using FDRWebsite.Client.Models.Objects;
using FDRWebsite.Client.Services;
using FDRWebsite.Shared.Helpers;
using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Pages
{
    public partial class Post
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public required PostService PostService { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public required Task<AuthenticationState> AuthStateTask { get; set; }

        private int? userID;

        private ICollection<string> thumbnailImages = new List<string>();

        private bool tooManyImages;

        private PostFormModel postModel = new();

        private EditContext? editContext;

        private ValidationMessageStore? validationMessageStore;

        protected override async Task OnInitializedAsync()
        {
            editContext = new(postModel);
            validationMessageStore = new(editContext);
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthStateTask;

            if (authenticationState.User.Identity?.IsAuthenticated == true)
            {
                var stringUserId = authenticationState.User.FindFirst(UserClaimTypes.ID)!.Value;
                if(int.TryParse(stringUserId, out int userID))
                    this.userID = userID;
            }
        }

        private async void SendPostAsync()
        {
            if (userID == null || !await ValidateEditContextAsync()) return;

            var wasSuccessfull = await PostService.AddNewPostAsync(new PostAddModel()
            { 
                Text = postModel.Text!,
                UserID = userID.Value,
                Images = postModel.Images,
            });

            if (wasSuccessfull)
                NavigationManager.NavigateTo("/");
        }   

        private async Task InputFileChanged(InputFileChangeEventArgs e)
        {
            tooManyImages = false;
            postModel.Images = null;
            thumbnailImages.Clear();

            if (e.FileCount > 10)
            {
                tooManyImages = true;
                StateHasChanged();
                return;
            }

            postModel.Images = e.GetMultipleFiles().ToList();

            if (postModel.Images == null) return;

            CheckFormField(nameof(postModel.Images));

            foreach(var image in postModel.Images)
                thumbnailImages.Add(await FileHelpers.GetThumbnailFromFileAsync(image));

            StateHasChanged();
        }

        private async Task<bool> ValidateEditContextAsync()
        {
            if (editContext == null) return false;

            return CheckFormField(nameof(postModel.Text)) &&
                   CheckFormField(nameof(postModel.Video)) &&
                   CheckFormField(nameof(postModel.Images)) &&
                   CheckFormField(nameof(postModel.Tags));
        }

        private bool CheckFormField(string fieldName)
        {
            if (editContext == null) return false;

            FieldIdentifier fieldIdentifier = editContext.Field(fieldName);
            editContext.NotifyFieldChanged(fieldIdentifier);
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if(!isValid) return false;

            if (fieldName == nameof(postModel.Images) && !ValidateAndRemoveInvalidImages())
            {
                validationMessageStore?.Add(fieldIdentifier, $"Files with the wrong format were removed");
            }

            return true;
        }

        private bool ValidateAndRemoveInvalidImages()
        {
            if(postModel.Images == null) return true;

            bool result = true;

            foreach (var image in postModel.Images)
            {
                if (!FileFormats.IsSupportedImage(image.Name))
                {
                    postModel.Images.Remove(image);
                    return false;
                }
            }

            return result;
        }
    }
}