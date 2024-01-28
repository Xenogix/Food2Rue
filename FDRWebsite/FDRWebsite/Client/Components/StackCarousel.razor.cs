using FDRWebsite.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace FDRWebsite.Client.Components
{
    public partial class StackCarousel
    {
        private const int HEIGHT_SHIFT = 10;
        private const int RIGHT_SHIFT = 20;
        private const int MAX_STACK_SIZE = 5;

        [Parameter]
        public required IEnumerable<Media> Medias { get; set; }

        private int currentIndex = 0;

        private int GetTopIndex()
        {
            return Medias.Count() - 1;
        }

        private void MoveNext()
        {
            currentIndex = Math.Min(currentIndex + 1, Medias.Count() - 1);
            StateHasChanged();
        }

        private void MovePrevious()
        {
            currentIndex = Math.Max(currentIndex - 1, 0);
            StateHasChanged();
        }
    }
}