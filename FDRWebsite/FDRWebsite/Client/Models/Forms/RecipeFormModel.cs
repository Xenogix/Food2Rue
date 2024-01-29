using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace FDRWebsite.Client.Models.Forms
{
    public class RecipeFormModel
    {
        [Required]
        [MaxLength(255, ErrorMessage = "The name cannot be longer than 255 characters")]
        public string? Name { get; set; }

        public string? Country { get; set; }

        [Required]
        public DateTime? PrepTime { get; set; }

        public DateTime? CookingTime { get; set; }

        public DateTime? RestTime { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "The steps cannot be longer than 1023 characters")]
        public string? Steps { get; set; }

        [MaxLength(10, ErrorMessage = "The recipe cannot contain more than 10 images")]
        public ICollection<IBrowserFile>? Images { get; set; }

        [Required]
        public ICollection<Ingredient>? Ingredients { get; set; }
    }
}
