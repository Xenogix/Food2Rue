using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models.Objects
{
    public struct AimeKey : IEquatable<AimeKey>
    {
        public int IdPublication { get; set; }

        public int IdUtilisateur { get; set; }

        public bool Equals(AimeKey other)
        {
            return IdPublication == other.IdPublication && IdUtilisateur == other.IdUtilisateur;
        }
    }

    public class Aime : IIdentifiable<AimeKey>
    {
        public AimeKey ID => new AimeKey() { IdUtilisateur = fk_utilisateur, IdPublication = fk_publication };

        public int fk_publication { get; set; }

        public int fk_utilisateur { get; set; }
    }
}
