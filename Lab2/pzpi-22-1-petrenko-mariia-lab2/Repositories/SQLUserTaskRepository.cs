using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLUserTaskRepository : IUserTaskRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLUserTaskRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddUserTasksAsync(List<UserTask> userTasks)
        {
            dbContext.UserTasks.AddRange(userTasks);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserTask> CreateAsync(UserTask userTask)
        {
            await dbContext.UserTasks.AddAsync(userTask);
            await dbContext.SaveChangesAsync();
            return userTask;
        }

        public async Task<UserTask?> DeleteAsync(Guid id)
        {
            var userTask = await dbContext.UserTasks.FindAsync(id);
            if (userTask == null)
            {
                return null;
            }

            dbContext.UserTasks.Remove(userTask);
            await dbContext.SaveChangesAsync();
            return userTask;
        }

        public async Task<List<UserTask>> GetAllAsync()
        {
            return await dbContext.UserTasks
                //.Include(ut => ut.User)
                //.Include(ut => ut.Assignment)
                .ToListAsync();
        }

        public async Task<UserTask?> GetByIdAsync(Guid id)
        {
            return await dbContext.UserTasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<UserTask>> GetByUserIdAsync(Guid userId)
        {
            return await dbContext.UserTasks
                .Where(ut => ut.UserId == userId)
                .ToListAsync();
        }
    }
}
