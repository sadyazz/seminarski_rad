namespace eReservation.DTO
{
    public class UserUpdateDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime DateBirth { get; set; }
        public string? ProfileImage { get; set; }
        public string? NewPassword { get; set; }
        public string? Username { get; set; }
    }
}
