using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Globalization;

namespace Calories.APP.Domain
{
    public class CaloriesDbFactory : IDesignTimeDbContextFactory<CaloriesDb>
    {
        const string CONNECTIONSTRING = "data source=CaloriesDB.db";

        public CaloriesDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CaloriesDb>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new CaloriesDb(optionsBuilder.Options);
        }

        public void SeedDb()
        {
            var db = CreateDbContext(null);

            var mealFoods = db.MealFoods.ToList();
            db.MealFoods.RemoveRange(mealFoods);
            var meals = db.Meals.ToList();
            db.Meals.RemoveRange(meals);
            var foods = db.Foods.ToList();
            db.Foods.RemoveRange(foods);
            var categories = db.Categories.ToList();
            db.Categories.RemoveRange(categories);

            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='MealFoods'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Meals'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Foods'");
            db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE " +
                "SET SEQ=0 WHERE NAME='Categories'");

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
            db.SaveChanges();

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
