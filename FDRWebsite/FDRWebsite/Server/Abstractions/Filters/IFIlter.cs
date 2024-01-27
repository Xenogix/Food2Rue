using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Server.Abstractions.Filters
{
    public interface IFilter
    {
        string GetFilterSQL();
        Dictionary<string, object> GetFilterParameters();

    }

    public interface IFilter<ParametersType> : IFilter
    {
        ParametersType? GetParameters();
        void SetParameters(ParametersType parameters);
    }
}
