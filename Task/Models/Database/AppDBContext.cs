using Microsoft.EntityFrameworkCore;
using Task.Models.Entities;

namespace Task.Models.Database
{
    public class AppDBContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }


        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>().HasData(
                new { Id = 1, Name = "MainStore" }
                );

            modelBuilder.Entity<Item>().HasData(
                new { Id = 2, Name = "MainProduct", Description = "the main product item" }
                );

            modelBuilder.Entity<StoreItem>()
               .HasKey(si => new { si.StoreId, si.ItemId });

            modelBuilder.Entity<StoreItem>()
                .HasOne(si => si.Store)
                .WithMany(s => s.StoreItems)
                .HasForeignKey(si => si.StoreId);

            modelBuilder.Entity<StoreItem>()
                .HasOne(si => si.Item)
                .WithMany(i => i.StoreItems)
                .HasForeignKey(si => si.ItemId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
