using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class User : IIdentifiable<int>
    {
        public int ID { get; set; }

        public required String FirstName { get; set; }

        public required String LastName { get; set; }

        public required String Email { get; set; }

        public required Image Profile_photo {  get; set; }

        public required String Pseudo {  get; set; }

        public required String Password { get; set; }

        public required DateOnly BirthDate { get; set; }

        public required DateTime Profile_Creation {  get; set; }

        public required String Description { get; set; }

        public required Pays Country { get; set; }

        public Publication[]? Publications { get; set; }

        public Recette[]? Recettes { get; set; }

        public Publication[]? Aime { get; set; }

    }

    public class Administrateur 
    {
    }


}
