using FDRWebsite.Server.Abstractions.Filters;

namespace FDRWebsite.Server.Filters
{
    public class EmptyFilter : IFilter<EmptyFilterParameters>
    {
        public Dictionary<string, object> GetFilterParameters()
        {
            return new();
        }

        public string GetFilterSQL()
        {
            return " TRUE";
        }

        public EmptyFilterParameters? GetParameters()
        {
            return null;
        }

        public void SetParameters(EmptyFilterParameters parameters)
        {
            // Do nothing
        }
    }
}
