using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    public class PaymentMethods
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }


    }
}
