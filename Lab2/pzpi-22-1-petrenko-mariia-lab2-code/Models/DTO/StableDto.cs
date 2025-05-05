namespace FarmKeeper.Models.DTO
{
    public class StableDto
    {
        public Guid Id { get; set; }
        public int MinFeedLevel { get; set; }
        public Guid FarmId { get; set; }
        public List<AnimalDto> Animals { get; set; }
        public List<FeedLevelHistory> FeedLevelHistory { get; set; }
    }
}
