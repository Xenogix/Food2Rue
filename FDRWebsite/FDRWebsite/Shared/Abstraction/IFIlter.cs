namespace FDRWebsite.Shared.Abstraction
{
    public interface IFilter<ModelType>
    {
        string GetFilterSQL();
    }
}
