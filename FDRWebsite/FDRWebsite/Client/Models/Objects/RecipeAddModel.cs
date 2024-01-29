using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Models.Objects
{
    public class RecipeAddModel
    {
        public required string Nom { get; set; }
        public required int Temps_Preparation { get; set; }
        public int? Temps_Cuisson { get; set; }
        public int? Temps_Repos { get; set; }
        public required string Etape { get; set; }
        public required int Fk_Utilisateur { get; set; }
        public IEnumerable<IBrowserFile>? Images { get; set; }
        public IEnumerable<Ingredient>? Ingredients { get; set; }
    }
}
