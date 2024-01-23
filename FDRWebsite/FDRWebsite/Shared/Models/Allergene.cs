using FDRWebsite.Shared.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FDRWebsite.Shared.Models
{
    public class Allergene : IIdentifiable<String>
    {
        public required String ID { get; set; }
    }
}