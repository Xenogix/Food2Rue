using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Allergene : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Nom { get; set; }
    }
}