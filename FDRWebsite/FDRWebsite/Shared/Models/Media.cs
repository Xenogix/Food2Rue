using System;
using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class Media : IIdentifiable<int>
    {
        public int ID { get; set; }

        public string URL_Source { get; set; }
    }

    public class Image : Media
    {
    }
    public class Video : Media
    {
    }
}
