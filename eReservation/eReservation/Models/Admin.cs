using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    [Table("Admin")]
    public class Admin:KorisnickiNalog
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
