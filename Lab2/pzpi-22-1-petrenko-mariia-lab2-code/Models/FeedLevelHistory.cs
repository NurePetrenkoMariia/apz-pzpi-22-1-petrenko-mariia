namespace FarmKeeper.Models
{
    public class FeedLevelHistory
    {
        public Guid Id { get; set; }
        public Guid StableId { get; set; }
        public Stable Stable { get; set; }
        public int FeedLevel { get; set; }
        public DateTime Timestamp { get; set; }
        public int? PredictedTimeToEmpty { get; set; }
    }
}
