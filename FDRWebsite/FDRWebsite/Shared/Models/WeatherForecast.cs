using FDRWebsite.Shared.Abstraction;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDRWebsite.Shared.Models
{
    public class WeatherForecast : IIdentifiable<int>
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("date")]
        public DateOnly Date { get; set; }

        [Column("temperature")]
        public int TemperatureC { get; set; }

        [Column("summary")]
        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}