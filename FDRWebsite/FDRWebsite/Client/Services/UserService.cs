using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Objects;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;

namespace FDRWebsite.Client.Services
{
    public class UserService
    {
        private IUtilisateurClient utilisateurClient { get; set; }

        private IPaysClient paysClient { get; set; }

        private ImageService imageService { get; set; }

        public UserService(IUtilisateurClient utilisateurClient, IPaysClient paysClient, ImageService imageService)
        {
            this.utilisateurClient = utilisateurClient;
            this.paysClient = paysClient;
            this.imageService = imageService;
        }

        public async Task<bool> AddNewUserAsync(UserAddModel userAddModel)
        {
            var image = await imageService.UploadImageAsync(userAddModel.ProfileImage);
            if (image == null) return false;

            var paysToSet = await paysClient.GetAsync(userAddModel.Country);
            if (paysToSet == null) return false;

            var resultUserID = await utilisateurClient.InsertAsync(new Utilisateur()
            {
                Prenom = userAddModel.FirstName,
                Nom = userAddModel.LastName,
                Email = userAddModel.Email,
                Pseudo = userAddModel.Nickname,
                Password = userAddModel.Password,
                Date_Naissance = userAddModel.BirthDate,
                Description = userAddModel.Description,
                Pays = paysToSet,
                Photo_Profil = image,
            });

            return resultUserID != null;
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
