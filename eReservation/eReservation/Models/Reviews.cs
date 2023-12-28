using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Reviews
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

        public int PropertiesID { get; set; }
        [ForeignKey(nameof(PropertiesID))]
        public Properties Properties { get; set; } 

        public int Review {  get; set; }
        public string Comment { get; set; }
        public DateTime DateReview { get; set; }
    }
}
