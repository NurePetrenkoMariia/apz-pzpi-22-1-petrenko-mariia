using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLStableRepository : IStableRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLStableRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Stable> CreateAsync(Stable stable)
        {
            await dbContext.Stables.AddAsync(stable);
            await dbContext.SaveChangesAsync();
            return stable;
        }

        public async Task<Stable?> DeleteAsync(Guid id)
        {
            var existingStable = await dbContext.Stables.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStable == null)
            {
                return null;
            }

            dbContext.Stables.Remove(existingStable);
            await dbContext.SaveChangesAsync();
            return existingStable;
        }

        public async Task<List<Stable>> GetAllAsync()
        {
            return await dbContext.Stables.Include(a => a.Animals).ToListAsync();
        }

        public async Task<Stable?> GetByIdAsync(Guid id)
        {
            return await dbContext.Stables
                .Include(a => a.Animals)
                .Include(s => s.Farm)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stable?> UpdateAsync(Guid id, Stable stable)
        {
            var existingStable = await dbContext.Stables.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStable == null)
            {
                return null;
            }

            existingStable.MinFeedLevel = stable.MinFeedLevel;
            existingStable.FarmId = stable.FarmId;

            await dbContext.SaveChangesAsync();
            return existingStable;
        }
    }
}
