using FarmKeeper.Enums;

namespace FarmKeeper.Models.DTO
{
    public class RegisterUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
