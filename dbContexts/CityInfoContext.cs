using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City("New York")
                {
                    Id = 1,
                    Description = "Description for this city."
                }
            );
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Centeral Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "Description for this point of interest."
                },
                new PointOfInterest("Park Centeral")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "Description for this point of interest."
                }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product("Skohylla", 299)
                {
                    Id = 2, 
                    InventoryCount = 1
                },
                new Product("Bordslampa", 600)
                {
                    Id = 2, 
                    InventoryCount = 4
                },
                new Product("Skohorn", 69.90m)
                {
                    Id = 2, 
                    InventoryCount = 100
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }

}