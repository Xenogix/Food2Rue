using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public abstract class Ajoutable : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Nom { get; set; }
        public required String Description { get; set; }
        public required DateOnly Date_Publication { get; set; }
        public required Image Image { get; set; }
        public required int Fk_Utilisateur { get; set; }
        public int? Fk_Administrateur { get; set; }
        public bool? Est_Valide { get; set; }
    }

    public class Ingredient : Ajoutable
    {
    }
    public class Ustensile : Ajoutable
    {
    }
}
