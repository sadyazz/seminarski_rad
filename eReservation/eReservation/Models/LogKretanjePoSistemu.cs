using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace eReservation.Models
{
    public class LogKretanjePoSistemu
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public KorisnickiNalog User { get; set; }
        public string? QueryPath { get; set; }
        public string? PostData { get; set; }
        public DateTime Time { get; set; }
        public string? IpAddress { get; set; }
        public string? ExceptionMessage { get; set; }
        public bool IsException { get; set; }
    }
}
