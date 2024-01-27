using FDRWebsite.Shared.Abstraction;
using System.Security.Cryptography;

namespace FDRWebsite.Shared.Models
{
    public class Tag : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required String Nom { get; set; }

        // override object.Equals
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj == null || GetType() != obj.GetType())
                return false;
            return ID.Equals(((Tag)obj).ID) && Nom.Equals(((Tag)obj).Nom);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ID^2 * Nom.Length;
        }
    }
}