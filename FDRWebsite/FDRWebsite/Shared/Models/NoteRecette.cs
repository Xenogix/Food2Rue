using FDRWebsite.Shared.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDRWebsite.Shared.Models
{
    public class NoteRecette : IIdentifiable<int>
    {
        public int ID => Utilisateur;
        public int Utilisateur { get; set; }
        public int Recette { get; set; }
        public int Note { get; set; }
    }
}
