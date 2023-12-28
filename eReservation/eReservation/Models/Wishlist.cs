using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Wishlist
    {
        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        public int PropertiesID {  get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; }

        public DateTime DateAdd { get; set; }
    }
}
