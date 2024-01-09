using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Pays : IIdentifiable<int>
    {
        public int ID { get; set; }
        public String sigle { get; set; }

        public String nom { get; set; }

        public User[]? Utilisateurs { get; set; }

        public Recette[]? Recettes { get; set; }
    }
}
