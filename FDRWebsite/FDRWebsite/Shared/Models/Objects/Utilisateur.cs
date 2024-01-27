using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Utilisateur : IIdentifiable<int>
    {
        public int ID { get; set; }

        public required string Prenom { get; set; }

        public required string Nom { get; set; }

        public required string Email { get; set; }

        public required Image Photo_Profil {  get; set; }

        public required string Pseudo {  get; set; }

        public required string Password { get; set; }

        public required DateTime Date_Naissance { get; set; }
        
        public required DateTime Date_Creation_Profil {  get; set; }

        public required string Description { get; set; }

        public required Pays Pays { get; set; }
    }
}
