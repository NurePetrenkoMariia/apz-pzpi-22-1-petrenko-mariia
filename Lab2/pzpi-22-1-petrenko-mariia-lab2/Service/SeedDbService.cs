 using FarmKeeper.Data;
using FarmKeeper.Enums;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Service
{
    public class SeedDbService
    {
        private readonly FarmKeeperDbContext dbContext;
        public SeedDbService(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task SeedAdminUserAsync()
        {
            var adminEmail = "farmkeeperadmin@gmail.com";
            if (!await dbContext.Users.AnyAsync(u => u.Email == adminEmail))
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword("bc49f7e6a00d41d0");
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Admin",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(2004, 10, 27),
                    PhoneNumber = "1234567890",
                    Email = adminEmail,
                    PasswordHash = hashedPassword,
                    Role = UserRole.DatabaseAdmin
                };

                await dbContext.Users.AddAsync(adminUser);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
