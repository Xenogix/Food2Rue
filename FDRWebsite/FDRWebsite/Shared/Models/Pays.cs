using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Pays : IIdentifiable<string>
    {
        public string ID => Sigle;
        public required string Sigle { get; set; }
        public required string Nom { get; set; }

    }
}
