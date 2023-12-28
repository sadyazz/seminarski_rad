using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Country
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

    }
}
