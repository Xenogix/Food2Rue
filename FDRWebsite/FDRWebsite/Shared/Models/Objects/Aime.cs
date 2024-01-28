using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models.Objects
{
    public class AimeKey : IEquatable<AimeKey>
    {
        public int IdPublication { get; set; }

        public int IdUtilisateur { get; set; }

        public bool Equals(AimeKey? other)
        {
            if(other == null) return false;
            return IdPublication == other.IdPublication && IdUtilisateur == other.IdUtilisateur;
        }
    }

    public class Aime : IIdentifiable<AimeKey>
    {
        public AimeKey ID => new AimeKey() { IdUtilisateur = IdUtilisateur, IdPublication = IdPublication };

        public int IdPublication { get; set; }

        public int IdUtilisateur { get; set; }
    }
}
