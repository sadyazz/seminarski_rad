using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Reservations
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        public int PropertiesID { get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; }

        public DateTime DateOfArrival { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public string Status { get; set; }

        public int PaymentMethodsID { get; set; }
        [ForeignKey(nameof(PaymentMethodsID))]
        public PaymentMethods PaymentMethods { get; set;}

    }
}
