namespace FDRWebsite.Shared.Abstraction
{
    public interface IIdentifiable<KeyType> where KeyType : IEquatable<KeyType>
    {
        public KeyType ID { get; }
    }
}
