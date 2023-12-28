using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class CalendarAvailability
    {
        [Key]
        public int ID { get; set; }
        public int PropertiesID { get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; }

        public DateTime Date {  get; set; }

        public string Availability {  get; set; }
    }
}
