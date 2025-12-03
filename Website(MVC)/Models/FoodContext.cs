using Microsoft.EntityFrameworkCore;

namespace Website_MVC_.Models
{
    public class FoodContext : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        
        public FoodContext(DbContextOptions<FoodContext> options) : base(options)
        {
        }
    }
}
