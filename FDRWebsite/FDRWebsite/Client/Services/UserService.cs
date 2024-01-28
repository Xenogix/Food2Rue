using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Objects;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using Refit;

namespace FDRWebsite.Client.Services
{
    public class UserService
    {
        private const int MAX_PROFILE_SIZE = 10000000;

        private IUtilisateurClient utilisateurClient { get; set; }

        private IImageClient imageClient { get; set; }

        private IFileClient fileClient { get; set; }

        private IPaysClient paysClient { get; set; }

        public UserService(IUtilisateurClient utilisateurClient, IImageClient imageClient, IFileClient fileClient, IPaysClient paysClient)
        {
            this.utilisateurClient = utilisateurClient;
            this.imageClient = imageClient;
            this.fileClient = fileClient;
            this.paysClient = paysClient;
        }

        public async Task<bool> AddNewUserAsync(UserAddModel userAddModel)
        {
            var browserFile = userAddModel.ProfileImage;
            var convertedFile = new StreamPart(browserFile.OpenReadStream(10000000), browserFile.Name, browserFile.ContentType);
            var uploadedFilePath = await fileClient.UploadAsync(convertedFile);
            if (uploadedFilePath == null) return false;

            var imageToInsert = new Image() { URL_Source = uploadedFilePath };
            var insertedImageID = await imageClient.InsertAsync(imageToInsert);
            if(insertedImageID == null) return false;
            imageToInsert.ID = insertedImageID;

            var paysToSet = await paysClient.GetAsync(userAddModel.Country);
            if (paysToSet == null) return false;

            var insertedUserID = await utilisateurClient.InsertAsync(new Utilisateur()
            {
                Prenom = userAddModel.FirstName,
                Nom = userAddModel.LastName,
                Email = userAddModel.Email,
                Pseudo = userAddModel.Nickname,
                Password = userAddModel.Password,
                Date_Naissance = userAddModel.BirthDate,
                Description = userAddModel.Description,
                Pays = paysToSet,
                Photo_Profil = imageToInsert,
            });

            if(insertedUserID == null) return false;

            return true;
        }

        public async Task<bool> IsEmailAlreadyTakenAsync(string? email)
        {
            if (email == null) return false;

            var parameters = new UtilisateurParameters() { Email = email.ToLower() };
            var users = await utilisateurClient.GetFilteredAsync(parameters);
            return users.Any();
        }
    }
}
