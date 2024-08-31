using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eReservation.Models
{
    public class Images
    {
        [Key]
        public int ID { get; set; }
        public string Path { get; set; }
        public int PropertyId {  get; set; }
        [ForeignKey(nameof(PropertyId))]
        public Properties Property { get; set; }

    }
}
