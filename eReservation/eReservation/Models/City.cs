
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class City
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public int CountryID { get; set; }
        [ForeignKey(nameof(CountryID))]

        public Country Country { get; set; }

    }
}
