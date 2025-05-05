namespace FarmKeeper.Models
{
    public class Stable
    {
        public Guid Id { get; set; }
        public int MinFeedLevel { get; set; }
        public Guid FarmId { get; set; }
        public Farm Farm { get; set; }
        public ICollection<Animal> Animals { get; set; }
        public ICollection<FeedLevelHistory> FeedLevelHistory { get; set; }
    }
}
