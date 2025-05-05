using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLNotificationRepository : INotificationRepository
    {

        private readonly FarmKeeperDbContext dbContext;
        public SQLNotificationRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> DeleteAsync(Guid id)
        {
            var existingNotification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);

            if (existingNotification == null)
            {
                return null;
            }

            dbContext.Notifications.Remove(existingNotification);
            await dbContext.SaveChangesAsync();
            return existingNotification;
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await dbContext.Notifications.ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Notification>> GetByUserIdAsync(Guid userId)
        {
            return await dbContext.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task<Notification?> UpdateAsync(Guid id, Notification notification)
        {
            var existingNotification = await dbContext.Notifications.FirstOrDefaultAsync(x => x.Id == id);

            if (existingNotification == null)
            {
                return null;
            }

            existingNotification.Title = notification.Title;
            existingNotification.Text = notification.Text;
            existingNotification.UserId = notification.UserId;

            await dbContext.SaveChangesAsync();
            return existingNotification;
        }


    }
}
