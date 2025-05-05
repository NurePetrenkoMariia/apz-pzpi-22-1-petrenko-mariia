namespace FarmKeeper.Models.DTO
{
    public class FarmDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public Guid OwnerId { get; set; }
        public List<StableDto> Stables { get; set; }
    }
}
