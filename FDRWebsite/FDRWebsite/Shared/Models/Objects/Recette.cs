using FDRWebsite.Shared.Abstraction;
using System.Runtime.InteropServices;

namespace FDRWebsite.Shared.Models
{
    public class Recette : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Nom { get; set; }
        public required int Temps_Preparation { get; set; }
        public int? Temps_Cuisson { get; set; }
        public int? Temps_Repos { get; set; }
        public required DateTime Date_Creation { get; set; }
        public required String Etape { get; set; }
        public required int Fk_Utilisateur { get; set; }
        public Video? Video { get; set; }
        public Pays? Pays { get; set; }
        public IEnumerable<Image>? Images { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
        public Dictionary<int, string>? Ingredients { get; set; }
        public IEnumerable<int>? Ustensiles { get; set; }
    }
}

