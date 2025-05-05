namespace FarmKeeper.Models.DTO
{
    public class FeedLevelHistoryDto
    {
        public Guid Id { get; set; }
        public Guid StableId { get; set; }
        public int FeedLevel { get; set; }
        public DateTime Timestamp { get; set; }
        public string? PredictedTimeToEmpty { get; set; }
    }
}
