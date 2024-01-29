using FDRWebsite.Client.Clients;
using FDRWebsite.Client.Models.Objects;
using FDRWebsite.Shared.Models;

namespace FDRWebsite.Client.Services
{
    public class RecipeService
    {
        private IRecetteClient recetteClient { get; set; }

        private ImageService imageService { get; set; }

        public RecipeService(IRecetteClient recetteClient, ImageService imageService)
        {
            this.recetteClient = recetteClient;
            this.imageService = imageService;
        }

        public async Task<bool> AddRecipeAsync(RecipeAddModel recipeAddModel)
        {
            throw new NotImplementedException();
            /*
            IEnumerable<Image> images;

            if (recipeAddModel.Images == null || !recipeAddModel.Images.Any())
                images = Array.Empty<Image>();
            else
                images = (await imageService.UploadImagesAsync(recipeAddModel.Images)).Where(i => i != null)!;

            var resultPublicationID = await recetteClient.InsertAsync(new Recette()
            {
                Nom = recipeAddModel.Nom,
                Temps_Preparation = recipeAddModel.Temps_Preparation,
                Temps_Cuisson = recipeAddModel.Temps_Cuisson,
                Temps_Repos = recipeAddModel.Temps_Repos,
                Etape = recipeAddModel.Etape,
                Fk_Utilisateur = recipeAddModel.Fk_Utilisateur,
                Images = images
            });

            return resultPublicationID != null;*/
        }
    }
}
