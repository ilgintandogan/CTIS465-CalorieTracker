using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Domain
{
    public class CaloriesDb : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<MealFood> MealFoods { get; set; }

        public CaloriesDb(DbContextOptions options) : base(options)
        {
        }
    }
}
