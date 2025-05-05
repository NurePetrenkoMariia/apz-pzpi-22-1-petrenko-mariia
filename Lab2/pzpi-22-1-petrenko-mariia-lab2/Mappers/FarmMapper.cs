using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class FarmMapper
    {
        public static FarmDto ToFarmDto(this Farm farmDomain)
        {
            return new FarmDto
            {
                Id = farmDomain.Id,
                Name = farmDomain.Name,
                Country = farmDomain.Country,
                City = farmDomain.City,
                Street = farmDomain.Street,
                OwnerId = farmDomain.OwnerId,
                Stables = farmDomain.Stables?.Select(s => s.ToStableDto()).ToList()
            };
        }

        public static Farm ToFarmFromCreate(this AddFarmRequestDto addFarmRequestDto, Guid ownerId)
        {
            return new Farm
            {
                Name = addFarmRequestDto.Name,
                Country = addFarmRequestDto.Country,
                City = addFarmRequestDto.City,
                Street = addFarmRequestDto.Street,
                OwnerId = ownerId,
            };
        }

        public static Farm ToFarmFromUpdate(this UpdateFarmRequestDto updateFarmRequestDto)
        {
            return new Farm
            {
                Name = updateFarmRequestDto.Name,
                Country = updateFarmRequestDto.Country,
                City = updateFarmRequestDto.City,
                Street = updateFarmRequestDto.Street,
                OwnerId = updateFarmRequestDto.OwnerId,
            };
        }
    }
}
