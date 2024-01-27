using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Components
{
    public partial class PostThread
    {
        [Parameter]
        public bool ReverseDirection { get; set; }

        [Parameter]
        public required IEnumerable<Publication> Posts { get; set; }
    }
}