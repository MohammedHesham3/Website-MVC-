using Microsoft.EntityFrameworkCore;
using Website_MVC_.Models;

namespace Website_MVC_.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // for testing
            modelBuilder.Entity<Food>().HasData(
                new Food 
                { 
                    FoodId = 1, 
                    Name = "Apple", 
                    ServingSize = 100, 
                    Calories = 52, 
                    Protein = 0.3f, 
                    Carbs = 13.8f, 
                    Fat = 0.2f 
                }
            );
        }
    }
}