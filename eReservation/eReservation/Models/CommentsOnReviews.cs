using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class CommentsOnReviews
    {
        [Key]
        public int ID { get; set; }

        public int ReviewID { get; set; }
        [ForeignKey(nameof(ReviewID))]
        public Reviews Reviews { get; set; }

        public int UserID { get; set; }
        [ForeignKey(nameof(UserID))]

        public User User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
