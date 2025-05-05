using FarmKeeper.Enums;
using System.ComponentModel.DataAnnotations;

namespace FarmKeeper.Models.DTO
{
    public class AddAssignmentRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Number of participants must be a non-negative integer.")]
        public int NumberOfParticipants { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Guid FarmId { get; set; }
    }
}
