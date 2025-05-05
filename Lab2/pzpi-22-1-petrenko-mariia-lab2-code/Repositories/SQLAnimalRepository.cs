using FarmKeeper.Data;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Repositories
{
    public class SQLAnimalRepository : IAnimalRepository
    {
        private readonly FarmKeeperDbContext dbContext;
        public SQLAnimalRepository(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Animal> CreateAsync(Animal animal)
        {
            await dbContext.Animals.AddAsync(animal);
            await dbContext.SaveChangesAsync();
            return animal;
        }

        public async Task<Animal?> DeleteAsync(Guid id)
        {
            var existingAnimal = await dbContext.Animals.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAnimal == null)
            {
                return null;
            }

            dbContext.Animals.Remove(existingAnimal);
            await dbContext.SaveChangesAsync();
            return existingAnimal;
        }

        public async Task<List<Animal>> GetAllAsync()
        {
            return await dbContext.Animals.ToListAsync();
        }

        public async Task<List<Animal>> GetByFarmIdAsync(Guid farmId)
        {
            var animalsWithFarmInfo = dbContext.Animals
                .Include(a => a.Stable)
                .ThenInclude(s => s.Farm);

            var animalsOfFarm = animalsWithFarmInfo
                .Where(a => a.Stable.Farm.Id == farmId);

            return await animalsOfFarm.ToListAsync();
        }

        public async Task<Animal?> GetByIdAsync(Guid id)
        {
            return await dbContext.Animals.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Animal>> GetByOwnerIdAsync(Guid ownerId)
        {
            var animalsWithFarmInfo = dbContext.Animals
                .Include(a => a.Stable)
                .ThenInclude(s => s.Farm);

            var animalsOfOwner = animalsWithFarmInfo
                .Where(a => a.Stable.Farm.OwnerId == ownerId);

            return await animalsOfOwner.ToListAsync();
        }

        public async Task<Animal?> UpdateAsync(Guid id, Animal animal)
        {
            var existingAnimal = await dbContext.Animals.FirstOrDefaultAsync(x => x.Id == id);

            if (existingAnimal == null)
            {
                return null;
            }

            existingAnimal.Name = animal.Name;
            existingAnimal.Species = animal.Species;
            existingAnimal.Breed = animal.Breed;
            existingAnimal.DateOfBirth = animal.DateOfBirth;
            existingAnimal.Sex = animal.Sex;
            existingAnimal.StableId = animal.StableId;

            await dbContext.SaveChangesAsync();
            return existingAnimal;
        }
    }
}
