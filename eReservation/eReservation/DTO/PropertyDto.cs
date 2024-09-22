using static eReservation.Controllers.PropertiesController;

namespace eReservation.DTO
{
    public class PropertyDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int PricePerNight { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public int PropertyTypeID { get; set; }
        public string PropertyTypeName { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public decimal AverageRating { get; set; }
        public List<ImageDto> Images { get; set; }
    }
}
