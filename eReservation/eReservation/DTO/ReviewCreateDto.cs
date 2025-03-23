namespace eReservation.DTO
{
    public class ReviewCreateDto
    {
        public int UserID { get; set; }
        public int PropertiesID { get; set; }
        public int Review { get; set; }
        public string Comment { get; set; }
    }
}
