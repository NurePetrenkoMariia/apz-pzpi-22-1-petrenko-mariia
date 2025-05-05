using FarmKeeper.Enums;
using FarmKeeper.Migrations;
using FarmKeeper.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FarmKeeper.Data
{
    public class FarmKeeperDbContext: DbContext
    {
        public FarmKeeperDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Stable> Stables { get; set; }
        public DbSet<FeedLevelHistory> FeedLevelHistory { get; set; }
        public DbSet<User> Users{ get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .HasOne(u => u.Farm) 
                 .WithMany(f => f.Workers) 
                 .HasForeignKey(u => u.FarmId) 
                 .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Farm>()
                .HasOne(f => f.Owner) 
                .WithMany(u => u.Farms)
                .HasForeignKey(f => f.OwnerId)  
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.AdministeredFarm)
                .WithMany(f => f.Administrators)
                .HasForeignKey(u => u.AdministeredFarmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Animal>()
                 .HasOne(a => a.Stable)
                 .WithMany(s => s.Animals)
                 .HasForeignKey(a => a.StableId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FeedLevelHistory>()
                .HasOne(flh => flh.Stable)
                .WithMany(s => s.FeedLevelHistory)
                .HasForeignKey(flh => flh.StableId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
