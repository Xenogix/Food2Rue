using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Models.Objects
{
    public class UserAddModel
    {
        public int ID { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required IBrowserFile ProfileImage { get; set; }

        public required string Nickname { get; set; }

        public required string Password { get; set; }

        public required DateTime BirthDate { get; set; }

        public string? Description { get; set; }

        public required string Country { get; set; }
    }
}
