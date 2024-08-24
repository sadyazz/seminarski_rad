using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Rezervacija
    {
        public DateTime DateOfArrival { get; set; }
        public DateTime DateOfDeparture { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }

        public int UserId { get; set; }
        public int PaymentMethodsId { get; set; }
        public int PropertiesId { get; set; }   

    }
}
