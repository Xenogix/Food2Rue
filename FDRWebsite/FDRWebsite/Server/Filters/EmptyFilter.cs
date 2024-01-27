using FDRWebsite.Server.Abstractions.Filters;

namespace FDRWebsite.Server.Filters
{
    public class EmptyFilter : IFilter<EmptyFilter>
    {
        public Dictionary<string, object> GetFilterParameters()
        {
            return new();
        }

        public string GetFilterSQL()
        {
            return " TRUE";
        }

        public EmptyFilter? GetParameters()
        {
            return null;
        }

        public void SetParameters(EmptyFilter parameters)
        {
            // Do nothing
        }
    }
}
