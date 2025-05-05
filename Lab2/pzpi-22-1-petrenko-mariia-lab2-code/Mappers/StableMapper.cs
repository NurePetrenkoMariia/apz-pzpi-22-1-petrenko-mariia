using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class StableMapper
    {
        public static StableDto ToStableDto(this Stable stableDomain)
        {
            return new StableDto
            {
                Id = stableDomain.Id,
                MinFeedLevel = stableDomain.MinFeedLevel,
                //CurrentFeedLevel = stableDomain.CurrentFeedLevel,
                //DateTimeOfUpdate = stableDomain.DateTimeOfUpdate,
                FarmId = stableDomain.FarmId,
                Animals = stableDomain.Animals?.Select(a => a.ToAnimalDto()).ToList(),
                FeedLevelHistory = stableDomain.FeedLevelHistory?.ToList()
            };
        }

        public static Stable ToStableFromCreate(this AddStableRequestDto addStableRequestDto, Guid farmId)
        {
            return new Stable
            {
                MinFeedLevel = addStableRequestDto.MinFeedLevel,
                FarmId = farmId,
            };
        }

        public static Stable ToStableFromUpdate(this UpdateStableRequestDto updateStableRequestDto)
        {
            return new Stable
            {
                MinFeedLevel = updateStableRequestDto.MinFeedLevel,
                FarmId = updateStableRequestDto.FarmId,
            };
        }
    }
}
