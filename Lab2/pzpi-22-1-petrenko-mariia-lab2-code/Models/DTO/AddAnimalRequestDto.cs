using FarmKeeper.Enums;

namespace FarmKeeper.Models.DTO
{
    public class AddAnimalRequestDto
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Sex Sex { get; set; }

    }
}
