using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class PropertiesAmenities
    {
        public int PropertiesID { get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; }

        public int AmenitiesID { get; set; }
        [ForeignKey(nameof(AmenitiesID))]
        public Amenities Amenities { get; set; }

    }
}
