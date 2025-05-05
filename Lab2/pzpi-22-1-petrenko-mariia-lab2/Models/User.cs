using FarmKeeper.Enums;

namespace FarmKeeper.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public ICollection<Notification> Notifications { get; set; }

        // for worker
        public Guid? FarmId { get; set; }
        public Farm Farm { get; set; }

        // for owner
        public ICollection<Farm> Farms { get; set; }

        //for farm admin
        public Guid? AdministeredFarmId { get; set; }  
        public Farm AdministeredFarm { get; set; }
    }
}
