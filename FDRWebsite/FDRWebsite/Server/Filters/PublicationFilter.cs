using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using System.Text;

namespace FDRWebsite.Server.Filters
{
    public class PublicationFilter : IFilter<PublicationParameters>
    {
        public PublicationParameters? Parameters { get; set; }

        public Dictionary<string, object> GetFilterParameters()
        {
            var result = new Dictionary<string, object>();

            if (Parameters == null) return result;

            return result;
        }

        public string GetFilterSQL()
        {
            var result = new StringBuilder("TRUE ");

            if (Parameters == null) return string.Empty;

            if (Parameters.UserIDs != null) result.Append($"AND publication.fk_utilisateur IN ({string.Join(",", Parameters.UserIDs)}) ");
            if (!Parameters.IncludeComments) result.Append($"AND publication.fk_parent <> NULL ");

            return result.ToString();
        }

        public PublicationParameters? GetParameters()
        {
            return Parameters;
        }

        public void SetParameters(PublicationParameters parameters)
        {
            Parameters = parameters;
        }
    }
}
