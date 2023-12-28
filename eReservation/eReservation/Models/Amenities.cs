using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    public class Amenities
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PropertiesAmenities> PropertiesAmenities { get; set; }
    }
}
