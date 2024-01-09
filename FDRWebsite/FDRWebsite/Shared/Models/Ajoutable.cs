using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public abstract class Ajoutable : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String nom { get; set; }
        public required String description { get; set; }
        public required DateOnly date_publication { get; set; }
        public required Image Image { get; set; }
        public required User utilisateur { get; set; }
        public User? administrateur { get; set; }
        public bool? est_valide { get; set; }
        public Recette[]? recette { get; set; }
    }

    public class Ingredient : Ajoutable
    {
        public Allergene[]? allergenes { get; set; }
    }
    public class Ustensile : Ajoutable
    {

    }
}
