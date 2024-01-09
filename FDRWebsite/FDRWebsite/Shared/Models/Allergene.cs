using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Allergene : IIdentifiable<int>
    {
        public int ID { get; set; }
        public String nom { get; set; }

        public Ingredient[] ingredients { get; set; }
    }
}