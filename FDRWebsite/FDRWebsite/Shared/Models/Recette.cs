using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Recette : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Nom { get; set; }
        public required int Temps_Preparation { get; set; }
        public int? Temps_Cuisson { get; set; }
        public int? Temps_Repos { get; set; }
        public required DateOnly Date_Creation { get; set; }
        public required String Etape { get; set; }
        public required User Utilisateur { get; set; }
        public Video? Video { get; set; }
        public Pays? Pays { get; set; }
        public Image[]? Images { get; set; }
        public Dictionary<User, int>? Note { get; set; }

        public Tag[]? Tags { get; set; }

        public required Dictionary<Ingredient, int>[] Ingredients { get; set; }
        
        public Ustensile[]? Ustensiles { get; set; }
    }
}