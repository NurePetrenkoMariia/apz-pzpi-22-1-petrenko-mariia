using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(Guid id, User user);
        Task<User?> DeleteAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetWorkersForAssignmentsAsync(Guid farmId);
        Task<User> AddAdminToFarmAsync(User user);
        Task<IEnumerable<User>> GetUsersByOwnerIdAsync(Guid ownerId);
        Task<bool> IsUserFromOwnersFarm(Guid userId, Guid ownerId);

    }
}
