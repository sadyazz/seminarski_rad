using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    [Table("Admin")]
    public class Admin:KorisnickiNalog
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
