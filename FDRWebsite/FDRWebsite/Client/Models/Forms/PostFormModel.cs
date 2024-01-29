using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace FDRWebsite.Client.Models.Forms
{
    public class PostFormModel
    {
        [Required]
        [MaxLength(255, ErrorMessage = "The text cannot be longer than 255 characters")]
        public string? Text { get; set; }

        [MaxLength(10, ErrorMessage = "The post cannot contain more than 10 images")]
        public ICollection<IBrowserFile>? Images { get; set; }
    }
}
