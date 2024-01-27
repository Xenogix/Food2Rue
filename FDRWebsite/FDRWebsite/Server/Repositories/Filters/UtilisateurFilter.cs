using FDRWebsite.Shared.Abstraction;
using FDRWebsite.Shared.Models;
using System.Text;

namespace FDRWebsite.Server.Repositories.Filters
{
    public class UtilisateurFilter : IFilter<Utilisateur>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public Dictionary<string, object> GetFilterParameters()
        {
            var result = new Dictionary<string, object>();

            if(UserName != null) result.Add("username", UserName);
            if (Email != null) result.Add("email", Email);
            if (Password != null) result.Add("password", Password);

            return result;
        }

        public string GetFilterSQL()
        {
            var result = new StringBuilder("TRUE ");

            if(UserName != null) result.Append($"AND pseudo = @username ");
            if (Email != null) result.Append($"AND email = @email ");
            if(Password != null) result.Append("AND password = @password ");

            return result.ToString();
        }
    }
}
