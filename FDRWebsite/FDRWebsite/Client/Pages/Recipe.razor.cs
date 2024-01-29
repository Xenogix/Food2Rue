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
    public partial class Recipe
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public required PostService PostService { get; set; }

        [Inject]
        public required IIngredientClient IngredientClient { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        public required Task<AuthenticationState> AuthStateTask { get; set; }

        private int? userID;

        private ICollection<string> thumbnailImages = new List<string>();

        private ICollection<Ingredient> ingredientList = new List<Ingredient>();

        private bool tooManyImages;

        private RecipeFormModel recipeModel = new();

        private EditContext? editContext;

        private ValidationMessageStore? validationMessageStore;

        protected override async Task OnInitializedAsync()
        {
            recipeModel.Ingredients = new List<Ingredient>();
            editContext = new(recipeModel);
            validationMessageStore = new(editContext);
            ingredientList = (await IngredientClient.GetAsync()).ToList();
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

            //if (wasSuccessfull)
                //NavigationManager.NavigateTo("/");
        }   

        private void SelectIngredientChanged(ChangeEventArgs e)
        {
            if(!int.TryParse(e.Value as string, out var ingredientID)) return;
            if (recipeModel.Ingredients!.Any(i => i.ID == ingredientID)) return;
            var ingredient = ingredientList.Where(i => i.ID == ingredientID).FirstOrDefault();
            if (ingredient == null) return;
            recipeModel.Ingredients!.Add(ingredient);
        }

        private void RemoveIngredient(int id)
        {
            var ingredient = recipeModel.Ingredients!.Where(i => i.ID == id).FirstOrDefault();
            if (ingredient == null) return;
            recipeModel.Ingredients!.Remove(ingredient);
        }

        private async Task InputFileChanged(InputFileChangeEventArgs e)
        {
            tooManyImages = false;
            recipeModel.Images = null;
            thumbnailImages.Clear();

            if (e.FileCount > 10)
            {
                tooManyImages = true;
                StateHasChanged();
                return;
            }

            recipeModel.Images = e.GetMultipleFiles().ToList();

            if (recipeModel.Images == null) return;

            CheckFormField(nameof(recipeModel.Images));

            foreach(var image in recipeModel.Images)
                thumbnailImages.Add(await FileHelpers.GetThumbnailFromFileAsync(image));

            StateHasChanged();
        }

        private async Task<bool> ValidateEditContextAsync()
        {
            if (editContext == null) return false;

            return CheckFormField(nameof(recipeModel.Name)) &&
                   CheckFormField(nameof(recipeModel.Country)) &&
                   CheckFormField(nameof(recipeModel.PrepTime)) &&
                   CheckFormField(nameof(recipeModel.CookingTime)) &&
                   CheckFormField(nameof(recipeModel.RestTime)) &&
                   CheckFormField(nameof(recipeModel.Steps)) &&
                   CheckFormField(nameof(recipeModel.Images));
        }

        private bool CheckFormField(string fieldName)
        {
            if (editContext == null) return false;

            FieldIdentifier fieldIdentifier = editContext.Field(fieldName);
            editContext.NotifyFieldChanged(fieldIdentifier);
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if(!isValid) return false;

            if (fieldName == nameof(recipeModel.Images) && !ValidateAndRemoveInvalidImages())
            {
                validationMessageStore?.Add(fieldIdentifier, $"Files with the wrong format were removed");
            }

            return true;
        }

        private bool ValidateAndRemoveInvalidImages()
        {
            if(recipeModel.Images == null) return true;

            bool result = true;

            foreach (var image in recipeModel.Images)
            {
                if (!FileFormats.IsSupportedImage(image.Name))
                {
                    recipeModel.Images.Remove(image);
                    return false;
                }
            }

            return result;
        }
    }
}