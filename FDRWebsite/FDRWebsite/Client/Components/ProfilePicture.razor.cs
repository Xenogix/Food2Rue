using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Components
{
    public partial class ProfilePicture
    {
        [Parameter]
        public required string ImageURL { get; set; }
    }
}