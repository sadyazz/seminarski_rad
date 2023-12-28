using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Images
    {
        [Key]
        public int ID { get; set; }
        public int PropertiesID {  get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; }

    }
}
