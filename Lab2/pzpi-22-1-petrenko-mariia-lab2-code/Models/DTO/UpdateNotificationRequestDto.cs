namespace FarmKeeper.Models.DTO
{
    public class UpdateNotificationRequestDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeCreated { get; set; }

        public Guid UserId { get; set; }
    }
}
