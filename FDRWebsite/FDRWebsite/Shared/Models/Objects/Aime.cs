using FDRWebsite.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDRWebsite.Shared.Models.Objects
{
    public class Aime : IIdentifiable<int>
    {
        public int ID => int.Parse(string.Format("%d%d", IdPublication, IdUtilisateur));

        public int IdPublication { get; set; }

        public int IdUtilisateur { get; set; }
    }
}
