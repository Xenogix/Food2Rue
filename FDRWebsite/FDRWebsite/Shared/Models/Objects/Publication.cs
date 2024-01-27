using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Publication : IIdentifiable<int>
    {
        public int ID { get; set; }

        public int? Parent { get; set; }

        public required string Texte { get; set; }

        public required DateTime Date_Publication { get; set; }

        public required int FK_Utilisateur { get; set; }

        public Video? Video { get; set; }

        public int? Recette { get; set; }

        public int Aime { get; set; }

        public int Commentaires { get; set; }

        public IEnumerable<Tag>? Tags { get; set; }

        public IEnumerable<Image>? Images { get; set; }
    }
}
