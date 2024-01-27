using FDRWebsite.Server.Abstractions.Filters;
using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using FDRWebsite.Shared.Models.Filters;
using System.Text;

namespace FDRWebsite.Server.Filters
{
    public class UtilisateurFilter : IFilter<UtilisateurParameters>
    {
        public UtilisateurParameters? Parameters { get; set; }

        public Dictionary<string, object> GetFilterParameters()
        {

            var result = new Dictionary<string, object>();

            if (Parameters == null) return result;

            if (Parameters.UserName != null) result.Add("username", Parameters.UserName);
            if (Parameters.Email != null) result.Add("email", Parameters.Email);
            if (Parameters.Password != null) result.Add("password", Parameters.Password);

            return result;
        }

        public string GetFilterSQL()
        {
            var result = new StringBuilder("TRUE ");

            if (Parameters == null) return string.Empty;

            if (Parameters.UserName != null) result.Append($"AND utilisateur.pseudo = @username ");
            if (Parameters.Email != null) result.Append($"AND utilisateur.email = @email ");
            if (Parameters.Password != null) result.Append("AND utilisateur.password = @password ");

            return result.ToString();
        }

        public UtilisateurParameters? GetParameters()
        {
            return Parameters;
        }

        public void SetParameters(UtilisateurParameters parameters)
        {
            Parameters = parameters;
        }
    }
}
