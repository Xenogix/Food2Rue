using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models.Filters
{
    public class PublicationParameters
    {
        public IEnumerable<int>? UserIDs { get; set; }

        public bool IncludeComments { get; set; }
    }
}
