using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Models.Filters;
using System.Text;

namespace FDRWebsite.Server.Filters
{
    public class AimeFilter : IFilter<AimeParameters>
    {
        public AimeParameters? Parameters { get; set; }

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

            if (Parameters.PublicationIDs != null) result.Append($"AND publication.fk_publication IN ({string.Join(",", Parameters.PublicationIDs)}) ");

            return result.ToString();
        }

        public AimeParameters? GetParameters()
        {
            return Parameters;
        }

        public void SetParameters(AimeParameters parameters)
        {
            Parameters = parameters;
        }
    }
}
