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
        public IEnumerable<Media> Medias { get; set; } = new Media[]
        {
            new Media() { URL_Source = "https://natashaskitchen.com/wp-content/uploads/2023/04/Flan-Recipe-SQ.jpg"},
            new Media() { URL_Source = "https://www.recipetineats.com/wp-content/uploads/2016/09/Creme-Brulee_8-SQ.jpg?w=500&h=500&crop=1"},
            new Media() { URL_Source = "https://spanishsabores.com/wp-content/uploads/2023/08/Crema-Catalana-Featured.jpg"},
            new Media() { URL_Source = "https://static01.nyt.com/images/2017/12/13/dining/15COOKING-CREME-BRULEE1/15COOKING-CREME-BRULEE1-superJumbo.jpg"},
            new Media() { URL_Source = "https://i0.wp.com/www.onceuponachef.com/images/2009/08/pancakes-01.jpg?resize=760%2C1040&ssl=1"},
            new Media() { URL_Source = "https://thegreatbritishbakeoff.co.uk/wp-content/uploads/2023/10/TarteAuxPomme-1024-450.jpg"},
            new Media() { URL_Source = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR8ykdj8pqTMZNtP5nXvtscTXIDeuUwG7BqDw&usqp=CAU"},
        };

        private int currentIndex = 0;

        private int GetTopIndex()
        {
            return Math.Min(currentIndex + MAX_STACK_SIZE, Medias.Count() - 1);
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