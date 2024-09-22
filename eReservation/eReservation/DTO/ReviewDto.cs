namespace eReservation.DTO
{
    public class ReviewDto
    {
            public int ID { get; set; }
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string UserFullName { get; set; }
            public int PropertiesID { get; set; }
            public string PropertyName { get; set; }
            public int Review { get; set; }
            public string Comment { get; set; }
            public DateTime DateReview { get; set; }
    }
}
