using FDRWebsite.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDRWebsite.Shared.Models
{
    public class ObjectImage : IIdentifiable<int>
    {
        public required int ID { get; set; }

        public required IEnumerable<Image>? Images { get; set; }
    }
}

