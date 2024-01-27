using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Allergene : IIdentifiable<string>
    {
        public string ID => Nom;
        public required string Nom { get; set; }
    }
}