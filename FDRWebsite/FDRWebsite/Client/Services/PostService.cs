using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Objects;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Services
{
    public class PostService
    {

        private IPublicationClient publicationClient { get; set; }

        private ImageService imageService { get; set; }

        public PostService(IPublicationClient publicationClient, ImageService imageService)
        {
            this.publicationClient = publicationClient;
            this.imageService = imageService;
        }

        public async Task<bool> AddNewPostAsync(PostAddModel postAddModel)
        {
            IEnumerable<Image> images;

            if (postAddModel.Images == null || !postAddModel.Images.Any())
                images = Array.Empty<Image>();
            else
                images = (await imageService.UploadImagesAsync(postAddModel.Images)).Where(i => i != null)!;

            var resultPublicationID = await publicationClient.InsertAsync(new Publication()
            {
                Texte = postAddModel.Text,
                FK_Utilisateur = postAddModel.UserID,
                Images = images?.Where(i => i != null)
            });

            return resultPublicationID != null;
        }
    }
}
