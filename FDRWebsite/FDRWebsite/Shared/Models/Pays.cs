using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Pays : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Sigle { get; set; }
        public required String Nom { get; set; }
    }
}
