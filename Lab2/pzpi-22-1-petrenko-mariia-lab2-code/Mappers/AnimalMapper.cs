using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using System.Runtime.CompilerServices;

namespace FarmKeeper.Mappers
{
    public static class AnimalMapper
    {
        public static AnimalDto ToAnimalDto(this Animal animalDomain)
        {
            return new AnimalDto
            {
                Id = animalDomain.Id,
                Name = animalDomain.Name,
                Species = animalDomain.Species,
                Breed = animalDomain.Breed,
                DateOfBirth = animalDomain.DateOfBirth,
                Sex = animalDomain.Sex,
                StableId = animalDomain.StableId,
            };
        }

        public static Animal ToAnimalFromCreate(this AddAnimalRequestDto addAnimalRequestDto, Guid stableId)
        {
            return new Animal
            {
                Name = addAnimalRequestDto.Name,
                Species = addAnimalRequestDto.Species,
                Breed = addAnimalRequestDto.Breed,
                DateOfBirth = addAnimalRequestDto.DateOfBirth,
                Sex = addAnimalRequestDto.Sex,
                StableId = stableId,
            };
        }

        public static Animal ToAnimalFromUpdate(this UpdateAnimalRequestDto updateAnimalRequestDto)
        {
            return new Animal
            {
                Name = updateAnimalRequestDto.Name,
                Species = updateAnimalRequestDto.Species,
                Breed = updateAnimalRequestDto.Breed,
                DateOfBirth = updateAnimalRequestDto.DateOfBirth,
                Sex = updateAnimalRequestDto.Sex,
                StableId = updateAnimalRequestDto.StableId,
            };
        }
    }
}
