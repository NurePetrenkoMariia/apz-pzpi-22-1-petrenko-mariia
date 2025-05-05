using System.ComponentModel.DataAnnotations;

namespace FarmKeeper.Models.DTO
{
    public class UpdateStableRequestDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "MinFeedLevel must be a non-negative integer.")]
        public int MinFeedLevel { get; set; }

        [Required]
        public Guid FarmId { get; set; }
    }
}
