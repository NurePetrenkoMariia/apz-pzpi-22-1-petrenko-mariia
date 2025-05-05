namespace FarmKeeper.Models.DTO
{
    public class AddUserTaskRequestDto
    {
        public Guid UserId { get; set; }
        public Guid AssignmentId { get; set; }
    }
}
