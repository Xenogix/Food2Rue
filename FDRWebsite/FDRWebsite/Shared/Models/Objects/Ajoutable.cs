using FDRWebsite.Shared.Abstraction;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDRWebsite.Shared.Models
{
    public abstract class Ajoutable : IIdentifiable<int>
    {
        public int ID { get; set; }

        public required string Nom_Ajoutable { get; set; }

        public required string Description { get; set; }
        public required DateTime Date_Publication { get; set; }
        public required Image Image { get; set; }
        public required int Fk_Utilisateur { get; set; }
        public int? Fk_Administrateur { get; set; }
        public bool? Est_Valide { get; set; }
    }

    public class Ingredient : Ajoutable
    {
        public ICollection<Allergene> Allergenes { get; set; } = new List<Allergene>();
    }

    public class Ustensile : Ajoutable
    {
    }
}
