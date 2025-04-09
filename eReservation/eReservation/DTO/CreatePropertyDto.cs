namespace eReservation.DTO
{
    public class CreatePropertyDto
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public int NumberOfRooms { get; set; }

        public int NumberOfBathrooms { get; set; }

        public int PricePerNight { get; set; }

        public int CityID { get; set; }

        public int PropertyTypeID { get; set; }

        public List<int> AmenitiesIDs { get; set; }

        public int UserID { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
