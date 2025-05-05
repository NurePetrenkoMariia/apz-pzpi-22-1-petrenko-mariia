using System.ComponentModel.DataAnnotations;

namespace FarmKeeper.Models.DTO
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
