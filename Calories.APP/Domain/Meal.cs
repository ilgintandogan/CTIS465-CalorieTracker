using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calories.APP.Domain
{
    public class Meal : Record
    {
        [Required, StringLength(50)]
        public string Title { get; set; }

        public MealTypes MealType { get; set; }

        public DateTime MealDate { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public List<MealFood> MealFoods { get; set; }
            = new();

        [NotMapped]
        public List<int> FoodIds
        {
            get => MealFoods.Select(mealFoodEntity
                => mealFoodEntity.FoodId).ToList();

            set => MealFoods = value.Select(foodIdValue => new MealFood
            {
                FoodId = foodIdValue
            }).ToList();
        }
    }
}
