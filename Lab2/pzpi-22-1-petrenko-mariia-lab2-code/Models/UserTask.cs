namespace FarmKeeper.Models
{
    public class UserTask
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; }
    }
}
