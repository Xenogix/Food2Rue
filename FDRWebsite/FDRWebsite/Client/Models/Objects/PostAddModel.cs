using Microsoft.AspNetCore.Components.Forms;

namespace FDRWebsite.Client.Models.Objects
{
    public class PostAddModel
    {
        public required string Text { get; set; }
        public required int UserID { get; set; }
        public IEnumerable<IBrowserFile>? Images { get; set; }
    }
}
