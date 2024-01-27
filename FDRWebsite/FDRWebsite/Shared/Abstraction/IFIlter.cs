using System.Data;

namespace FDRWebsite.Shared.Abstraction
{
    public interface IFilter<ModelType>
    {
        string GetFilterSQL();
        Dictionary<string,object> GetFilterParameters();
    }
}
