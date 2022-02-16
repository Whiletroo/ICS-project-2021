using System;
using Microsoft.EntityFrameworkCore;
using Festival.DAL.Entity;
using Festival.DAL.Seeds;

namespace Festival.DAL
{
    public class FestivalDbContext : DbContext
    {
        public FestivalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BandEntity> Bands { get; set; } = null!;
        public DbSet<StageEntity> Stages { get; set; } = null!;
        public DbSet<PerformanceEntity> Performances { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BandEntity>()
                .HasMany<PerformanceEntity>(b => b.Performances)
                .WithOne(p => p.Band!)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StageEntity>()
                .HasMany<PerformanceEntity>(s => s.Performances)
                .WithOne(p => p.Stage!)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }
    }
}
