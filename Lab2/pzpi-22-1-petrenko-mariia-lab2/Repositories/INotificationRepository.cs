using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllAsync();
        Task<Notification?> GetByIdAsync(Guid id);
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification?> UpdateAsync(Guid id, Notification notification);
        Task<Notification?> DeleteAsync(Guid id);
        Task<List<Notification>> GetByUserIdAsync(Guid userId);
    }
}
