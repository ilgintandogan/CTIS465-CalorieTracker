using CORE.Domain;

namespace Calories.APP.Domain
{
    public class MealFood : Record
    {
        public int MealId { get; set; }

        public Meal Meal { get; set; }

        public int FoodId { get; set; }

        public Food Food { get; set; }
    }
}
