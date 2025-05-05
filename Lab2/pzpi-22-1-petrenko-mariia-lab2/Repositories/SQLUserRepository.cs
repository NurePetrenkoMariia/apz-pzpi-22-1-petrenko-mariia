using FarmKeeper.Data;
using FarmKeeper.Enums;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLUserRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> AddAdminToFarmAsync(User admin)
        {
            await dbContext.Users.AddAsync(admin);
            await dbContext.SaveChangesAsync();
            return admin;
        }

        public async Task<User> CreateAsync(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteAsync(Guid id)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return null;
            }

            dbContext.Users.Remove(existingUser);
            await dbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await dbContext.Users
                .Include(f => f.Farms)
                .ThenInclude(f => f.Stables)
                .ThenInclude(s => s.Animals)
                .Include(n => n.Notifications)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await dbContext.Users
                .Include(f => f.Farms)
                .ThenInclude(f => f.Stables)
                .ThenInclude(s => s.Animals)
                .Include(n => n.Notifications)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<User>> GetUsersByOwnerIdAsync(Guid ownerId)
        {
            return await dbContext.Users
                .Where(u => u.Farm.OwnerId == ownerId)
                .ToListAsync();
        }

        public async Task<List<User>> GetWorkersForAssignmentsAsync(Guid farmId)
        {
            return await dbContext.Users
                .Where(u => u.Role == UserRole.Worker && u.FarmId == farmId)
                .ToListAsync();
        }

        public async Task<bool> IsUserFromOwnersFarm(Guid userId, Guid ownerId)
        {
            return await dbContext.Users
                .AnyAsync(u => u.Id == userId && u.Farm.OwnerId == ownerId);
        }

        public async Task<User?> UpdateAsync(Guid id, User user)
        {
            var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return null;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.DateOfBirth = user.DateOfBirth;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.Role = user.Role;
            existingUser.FarmId = user.FarmId;
            existingUser.AdministeredFarmId = user.AdministeredFarmId;

            await dbContext.SaveChangesAsync();
            return existingUser;
        }
    }
}
