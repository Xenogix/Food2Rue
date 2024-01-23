using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Pages
{
    public partial class UserProfile
    {
        [Parameter]
        public int? UserId { get; set; }
    }
}