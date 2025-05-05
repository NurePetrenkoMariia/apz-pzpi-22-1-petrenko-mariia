using FarmKeeper.Models;
using FarmKeeper.Models.DTO;

namespace FarmKeeper.Repositories
{
    public interface IFeedMonitoringService
    {
        Task MonitorFeedLevelAsync( FeedLevelHistory feedLevelHistory);
        Task<List<FeedLevelHistoryDtoForIoT>> GetFeedHistoryAsync(Guid stableId);
        Task<List<FeedLevelHistory>> GetAllAsync();
        Task<List<FeedLevelHistory>> GetByOwnerIdAsync(Guid ownerId);
        Task<List<FeedLevelHistory>> GetByAdminIdAsync(Guid adminId);
    }
}
