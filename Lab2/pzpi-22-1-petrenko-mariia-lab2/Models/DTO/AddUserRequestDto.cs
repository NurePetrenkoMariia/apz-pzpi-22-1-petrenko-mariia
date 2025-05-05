using FarmKeeper.Enums;

namespace FarmKeeper.Models.DTO
{
    public class AddUserRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public Guid? FarmId { get; set; }
        public Guid? AdministeredFarmId { get; set; }
    }
}
