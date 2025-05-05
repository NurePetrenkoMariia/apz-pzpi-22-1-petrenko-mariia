namespace FarmKeeper.Models.DTO
{
    public class FeedUpdateDto
    {
        public Guid StableId { get; set; }
        public int CurrentFeedLevel { get; set; }
        public int? PredictedTimeToEmpty { get; set; }
    }
} 
