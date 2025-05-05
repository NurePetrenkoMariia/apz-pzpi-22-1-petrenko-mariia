using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface IStableRepository
    {
        Task<List<Stable>> GetAllAsync();
        Task<Stable?> GetByIdAsync(Guid id);
        Task<Stable> CreateAsync(Stable stable);
        Task<Stable?> UpdateAsync(Guid id, Stable stable);
        Task<Stable?> DeleteAsync(Guid id);
    }
}
