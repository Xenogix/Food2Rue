using System;
using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Media : IIdentifiable<int>
    {
        public int ID { get; set; }
        public required string URL_Source { get; set; }
    }

    public class Image : Media
    {
        // override object.Equals
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return ID.Equals(((Image)obj).ID) && URL_Source.Equals(((Image)obj).URL_Source);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ID ^ 2 * URL_Source.Length;
        }
    }

    public class Video : Media
    {
        // override object.Equals
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            return ID.Equals(((Image)obj).ID) && URL_Source.Equals(((Image)obj).URL_Source);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ID ^ 2 * URL_Source.Length;
        }
    }
}
