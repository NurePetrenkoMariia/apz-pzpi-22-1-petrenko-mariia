using FarmKeeper.Models.DTO;
using FarmKeeper.Models;

namespace FarmKeeper.Mappers
{
    public static class FeedLevelHistoryMapper
    {
        public static FeedLevelHistoryDto ToFeedLevelHistoryDto(this FeedLevelHistory feedLevelHistoryDomain)
        {
            string formattedPrediction = "null";
            if (feedLevelHistoryDomain.PredictedTimeToEmpty != null)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds((double)feedLevelHistoryDomain.PredictedTimeToEmpty);
                formattedPrediction = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            }
           
            return new FeedLevelHistoryDto
            {
                Id = feedLevelHistoryDomain.Id,
                StableId = feedLevelHistoryDomain.StableId,
                FeedLevel = feedLevelHistoryDomain.FeedLevel,
                Timestamp = feedLevelHistoryDomain.Timestamp,
                PredictedTimeToEmpty = formattedPrediction
            };
        }
    }
}
