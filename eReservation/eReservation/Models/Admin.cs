using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    public class Admin:KorisnickiNalog
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
