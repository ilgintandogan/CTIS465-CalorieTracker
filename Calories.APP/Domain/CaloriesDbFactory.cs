using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Globalization;

namespace Calories.APP.Domain
{
    // Provides a factory for creating CaloriesDb instances at design time.
    // This is used by Entity Framework (EF) tools such as
    // migrations to construct the database context when the
    // application is not running.
    // This class may need to be created if there are any exceptions
    // during Scaffolding.
    public class CaloriesDbFactory : IDesignTimeDbContextFactory<CaloriesDb>
    {
        // The connection string in the configuration file
        // (appsettings.json).
        const string CONNECTIONSTRING = "data source=CaloriesDB.db";

        // Creates a new instance of the CaloriesDb type using the connection
        // string. This method is called by EF tooling at design time.
        public CaloriesDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CaloriesDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new CaloriesDb(optionsBuilder.Options);
        }

        // Seeds the database tables with initial data (Optional).
        public void SeedDb()
        {
            // Create a new instance of the CaloriesDb type:
            var db = CreateDbContext(null);

            // Delete existing data:
            var mealFoods = db.MealFoods.ToList();
            db.MealFoods.RemoveRange(mealFoods);
            var meals = db.Meals.ToList();
            db.Meals.RemoveRange(meals);
            var foods = db.Foods.ToList();
            db.Foods.RemoveRange(foods);
            var categories = db.Categories.ToList();
            db.Categories.RemoveRange(categories);

            // Reset identity columns (for SQLite):
            // IDs will start from 1
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='MealFoods'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Meals'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Foods'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Categories'");

            // Insert new data:
            // Categories with Foods:
            db.Categories.Add(new Category
            {
                Name = "Fruits",
                Foods = new List<Food>
                {
                    new Food
                    {
                        Name = "Apple",
                        ServingDescription = "1 medium (182 g)",
                        Calories = 95,
                        ProteinGrams = 0.5m,
                        IsVegan = true,
                        CreatedDate = new DateTime(2026, 11, 9, 10, 0, 0)
                    },
                    new Food
                    {
                        Name = "Banana",
                        ServingDescription = "1 medium (118 g)",
                        Calories = 105,
                        ProteinGrams = 1.3m,
                        IsVegan = true,
                        CreatedDate = DateTime.Parse(
                            "11/09/2026 10:05:00", new CultureInfo("en-US"))
                    }
                }
            });
            db.Categories.Add(new Category
            {
                Name = "Dairy",
                Foods = new List<Food>
                {
                    new Food
                    {
                        Name = "Yogurt",
                        ServingDescription = "1 cup (245 g)",
                        Calories = 149,
                        ProteinGrams = 8.5m,
                        IsVegan = false,
                        CreatedDate = new DateTime(2026, 11, 9, 10, 10, 0)
                    }
                }
            });
            db.Categories.Add(new Category
            {
                Name = "Grains",
                Foods = new List<Food>
                {
                    new Food
                    {
                        Name = "Oatmeal",
                        ServingDescription = "1 cup cooked (234 g)",
                        Calories = 166,
                        ProteinGrams = 5.9m,
                        IsVegan = true,
                        CreatedDate = new DateTime(2026, 11, 9, 10, 15, 0)
                    }
                }
            });
            db.Categories.Add(new Category { Name = "Vegetables" });
            db.SaveChanges(); // commit changes to the database

            // Meals with MealFoods:
            db.Meals.Add(new Meal
            {
                Title = "Morning Bowl",
                MealType = MealTypes.Breakfast,
                MealDate = new DateTime(2026, 11, 9, 8, 30, 0),
                Notes = "Oatmeal with fruit and yogurt",
                MealFoods = new List<MealFood>
                {
                    new MealFood { FoodId = db.Foods.Single(
                        f => f.Name == "Oatmeal").Id },
                    new MealFood { FoodId = db.Foods.Single(
                        f => f.Name == "Banana").Id },
                    new MealFood { FoodId = db.Foods.Single(
                        f => f.Name == "Yogurt").Id }
                }
            });
            db.Meals.Add(new Meal
            {
                Title = "Afternoon Snack",
                MealType = MealTypes.Snack,
                MealDate = new DateTime(2026, 11, 9, 16, 0, 0),
                MealFoods = new List<MealFood>
                {
                    new MealFood { FoodId = db.Foods.Single(
                        f => f.Name == "Apple").Id }
                }
            });
            db.SaveChanges();
        }
    }
}
