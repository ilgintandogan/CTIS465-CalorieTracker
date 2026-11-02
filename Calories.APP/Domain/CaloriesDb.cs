using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Domain
{
    // Inherits from the Entity Framework's DbContext class for
    // database configurations and operations.
    public class CaloriesDb : DbContext
    {
        // Represents the Categories table in the database.
        public DbSet<Category> Categories { get; set; }

        // Represents the Foods table in the database.
        public DbSet<Food> Foods { get; set; }

        // Represents the Meals table in the database.
        public DbSet<Meal> Meals { get; set; }

        // Represents the MealFoods table in the database.
        public DbSet<MealFood> MealFoods { get; set; }

        // Constructor that takes options of type DbContextOptions
        // parameter and passes it to the base DbContext class constructor.
        // The options parameter is used to configure the database
        // connection with such as the connection string.
        public CaloriesDb(DbContextOptions options) : base(options)
        {
        }
    }
}
