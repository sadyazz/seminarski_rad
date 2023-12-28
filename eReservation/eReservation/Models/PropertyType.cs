using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    public class PropertyType
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
