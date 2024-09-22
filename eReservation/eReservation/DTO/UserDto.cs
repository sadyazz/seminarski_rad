namespace eReservation.DTO
{
    public class UserDto
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateJoined { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
