using FarmKeeper.Data;
using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.IO;

namespace FarmKeeper.Repositories
{
    public class SQLFarmRepository : IFarmRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLFarmRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Farm> CreateAsync(Farm farm)
        {
            await dbContext.Farms.AddAsync(farm);
            await dbContext.SaveChangesAsync();
            return farm;
        }

        public async Task<Farm?> DeleteAsync(Guid id)
        {
            var existingFarm = await dbContext.Farms.FirstOrDefaultAsync(x => x.Id == id);

            if (existingFarm == null)
            {
                return null;
            }

            dbContext.Farms.Remove(existingFarm);
            await dbContext.SaveChangesAsync();
            return existingFarm;
        }

        public async Task<List<Farm>> GetAllAsync()
        {
            return await dbContext.Farms.Include(u => u.Workers).Include(u => u.Administrators).Include(f => f.Stables).ThenInclude(s => s.Animals).ToListAsync();
        }

        public async Task<List<Farm>> GetFarmsByOwnerIdAsync(Guid ownerId)
        {
            return await dbContext.Farms
                .Where(farm => farm.OwnerId == ownerId)
                .Include(u => u.Workers)
                .Include(u => u.Administrators)
                .Include(f => f.Stables)
                .ThenInclude(s => s.Animals)
                .ToListAsync();
        }

        public async Task<Farm?> GetByIdAsync(Guid id)
        {
            return await dbContext.Farms.Include(u => u.Workers).Include(u => u.Administrators).Include(s => s.Stables).ThenInclude(s => s.Animals).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Farm?> UpdateAsync(Guid id, Farm farm)
        {
            var existingFarm = await dbContext.Farms.FirstOrDefaultAsync(x => x.Id == id);

            if (existingFarm == null)
            {
                return null;
            }

            existingFarm.Name = farm.Name;
            existingFarm.Country = farm.Country;
            existingFarm.City = farm.City;
            existingFarm.Street = farm.Street;
            existingFarm.OwnerId = farm.OwnerId;

            await dbContext.SaveChangesAsync();
            return existingFarm;
        }

        public async Task<Farm?> GetFarmByAdminIdAsync(Guid adminId)
        {
            return await dbContext.Farms
                .Include(f => f.Workers)
                .Include(f => f.Administrators)
                .Include(f => f.Stables)
                .ThenInclude(s => s.Animals)
                .FirstOrDefaultAsync(farm => farm.Administrators.Any(admin => admin.Id == adminId));
        }
    }
}
