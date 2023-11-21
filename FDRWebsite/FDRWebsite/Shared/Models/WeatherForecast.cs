using FDRWebsite.Shared.Abstraction;

namespace FDRWebsite.Shared.Models
{
    public class WeatherForecast : IIdentifiable<int>
    {
        public int ID { get; set; }

        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}