using FDRWebsite.Client.Authentication;
using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Forms;
using FDRWebsite.Client.Services;
using FDRWebsite.Shared.Helpers;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Pages
{
    public partial class Register
    {
        [Inject]
        public required AuthenticationService AuthenticationService { get; set; }

        [Inject]
        public required UserService UserService { get; set; }

        [Inject]
        public required IPaysClient PaysClient { get; set; }

        [Inject]
        public required NavigationManager NavigationManager { get; set; }

        private const int MAX_PROFILE_THUMBNAIL_SIZE = 1024;

        private string? profileThumbnailImage = null;

        private enum Steps
        {
            RequiredInformations,
            OptionalInformations, 
            UserProfile
        }

        private Steps CurrentStep;

        private Pays emptyCountry = new Pays() { Nom = string.Empty, Sigle = string.Empty };

        private IEnumerable<Pays> countries = Array.Empty<Pays>();

        private RegisterFormModel registerModel = new();

        private EditContext? editContext;

        private ValidationMessageStore? validationMessageStore;

        protected override async Task OnInitializedAsync()
        {
            editContext = new(registerModel);
            validationMessageStore = new(editContext);
            countries = (new Pays[] { emptyCountry }).Concat(await PaysClient.GetAsync());
            await base.OnInitializedAsync();
        }

        private async void RegisterAsync()
        {
            var wasSuccessfull = await UserService.AddNewUserAsync(new()
            {
                FirstName = registerModel.FirstName!,
                LastName = registerModel.LastName!,
                Nickname = registerModel.NickName!,
                Email = registerModel.Email!,
                Password = registerModel.Password!,
                BirthDate = registerModel.BirthDate!.Value,
                Country = registerModel.Country!,
                Description = registerModel.ProfileDescription,
                ProfileImage = registerModel.ProfileImage!
            });

            if (wasSuccessfull)
                NavigationManager.NavigateTo("login");
        }   

        private async void NextStepAsync() => await SetStepAsync(CurrentStep + 1);

        private async void PreviousStateAsync() => await SetStepAsync(CurrentStep - 1);

        private async Task SetStepAsync(Steps step)
        {
            if ((int)step > (int)CurrentStep && !await ValidateEditContextAsync()) return;
            CurrentStep = step;
            editContext = new EditContext(registerModel);
            StateHasChanged();
        }

        private async Task InputFileChangedAsync(InputFileChangeEventArgs e)
        {
            registerModel.ProfileImage = e.File;
            profileThumbnailImage = null;

            if (registerModel.ProfileImage == null) return;

            if(!await CheckFormFieldAsync(nameof(registerModel.ProfileImage)))
            {
                registerModel.ProfileImage = null;
            }
            else
            {
                profileThumbnailImage = await FileHelpers.GetThumbnailFromFileAsync(registerModel.ProfileImage);
            }

            StateHasChanged();
        }

        private async Task<bool> ValidateEditContextAsync()
        {
            if (editContext == null) return false;

            switch (CurrentStep)
            {
                case Steps.RequiredInformations:
                    return await CheckFormFieldAsync(nameof(registerModel.FirstName)) &&
                           await CheckFormFieldAsync(nameof(registerModel.LastName)) &&
                           await CheckFormFieldAsync(nameof(registerModel.Email)) &&
                           await CheckFormFieldAsync(nameof(registerModel.NickName)) &&
                           await CheckFormFieldAsync(nameof(registerModel.Password));

                case Steps.OptionalInformations:
                    return await CheckFormFieldAsync(nameof(registerModel.BirthDate)) &&
                           await CheckFormFieldAsync(nameof(registerModel.Country));

                case Steps.UserProfile:
                    return await CheckFormFieldAsync(nameof(registerModel.ProfileDescription)) &&
                           await CheckFormFieldAsync(nameof(registerModel.ProfileImage));

                default:
                    return true;
            }
        }

        private async Task<bool> CheckFormFieldAsync(string fieldName)
        {
            if (editContext == null) return false;

            FieldIdentifier fieldIdentifier = editContext.Field(fieldName);
            editContext.NotifyFieldChanged(fieldIdentifier);
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            if(!isValid) return false;

            if (fieldName == nameof(registerModel.Email) && await UserService.IsEmailAlreadyTakenAsync(registerModel.Email))
            {
                validationMessageStore?.Add(fieldIdentifier, "An account with the same email already exists");
                return false;
            }
            else if (fieldName == nameof(registerModel.ProfileImage) && !IsProfileImageValide(registerModel.ProfileImage))
            {
                validationMessageStore?.Add(fieldIdentifier, "The provided file format is not supported");
                return false;
            }

            return true;
        }

        private bool IsProfileImageValide(IBrowserFile? file)
        {
            if (file == null) return false;
            return FileFormats.IsSupportedImage(file.Name);
        }
    }
}