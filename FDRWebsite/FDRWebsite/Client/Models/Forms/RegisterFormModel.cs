using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace FDRWebsite.Client.Models.Forms
{
    public class RegisterFormModel
    {
        [Required(ErrorMessage = "Please fill this field")]
        [StringLength(64, ErrorMessage = "The provided first name is too long")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please fill this field")]
        [StringLength(64, ErrorMessage = "The provided last name is too long")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please fill this field")]
        [MinLength(3, ErrorMessage = "Your nickname must be at least 3 characters long")]
        [MaxLength(16, ErrorMessage = "Your nickname must be at most 16 characters long")]
        public string? NickName { get; set; }

        [Required(ErrorMessage = "Please fill this field")]
        [RegularExpression(@"(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-zA-Z0-9-]*[a-zA-Z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])",
            ErrorMessage = "The provided email is not valid")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please specify a password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Your password must contain at least one letter and one number and at least 6 characters long")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please select a country")]
        public string? Country { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Please fill this field")]
        [StringLength(255, ErrorMessage = "The provided description should be less thant 255 characters long")]
        public string? ProfileDescription { get; set; }

        [Required]
        public IBrowserFile? ProfileImage { get; set; }
    }
}
