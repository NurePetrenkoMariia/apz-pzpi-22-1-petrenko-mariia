using FarmKeeper.Enums;

namespace FarmKeeper.Models
{
    public class Animal
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Sex Sex { get; set; }
        public Guid StableId { get; set; }
        public Stable Stable { get; set; }
    }
}
