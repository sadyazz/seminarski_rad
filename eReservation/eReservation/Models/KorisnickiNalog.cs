using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace eReservation.Models
{
    [Table("KorisnickiNalog")]
    public class KorisnickiNalog
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public User? Korisnik => this as User;

        [JsonIgnore]
        public Admin? Administrator => this as Admin;

        public bool isAdmin { get; set; }
        public bool isUser { get; set; }

        public bool Is2FActive { get; set; }
    }
}
