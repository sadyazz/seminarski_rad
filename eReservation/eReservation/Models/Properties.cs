using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Properties
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        public string Name { get; set; }

        public string Adress { get; set; }

        public int NumberOfRooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public int PricePerNight { get; set; }

        public int CityID { get; set; }
        [ForeignKey(nameof(CityID))]
        public City City { get; set; }

        public int PropertyTypeID { get; set; }
        [ForeignKey(nameof(PropertyTypeID))]
        public PropertyType PropertyType { get; set; }

        public List<PropertiesAmenities> PropertiesAmenities { get; set; }
        //public List<Wishlist> Wishlist { get; set; }

        public ICollection<Images> Images { get; set; } = new List<Images>();
        public List<Reviews> Reviews { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

    }
}
