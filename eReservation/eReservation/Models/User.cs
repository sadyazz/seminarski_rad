using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    [Table("User")]
    public class User : KorisnickiNalog
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfRegistraion { get; set; }
        public DateTime DateBirth { get; set; }
        //public List<Wishlist> Wishlist { get; set; }
        //public string ProfileImage { get; set; }
    }
}
