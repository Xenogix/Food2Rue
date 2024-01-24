using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class PublicationTag : IIdentifiable<int>
    {
        public required int ID { get; set; }

        public required IEnumerable<Tag> Tags { get; set; }
    }
}
