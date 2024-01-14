using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Utilisateur : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Prénom { get; set; }

        public required String Nom { get; set; }

        public required String Email { get; set; }

        public required Image Photo_Profil {  get; set; }

        public required String Pseudo {  get; set; }

        public required String Password { get; set; }

        public required DateTime Date_Naissance { get; set; }
        
        public required DateTime Date_Creation_Profil {  get; set; }

        public required String Description { get; set; }

        public required Pays Pays { get; set; }

    }
}
