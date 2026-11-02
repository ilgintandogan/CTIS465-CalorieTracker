using CORE.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calories.APP.Domain
{
    // Meal is a Record.
    public class Meal : Record
    {
        // Reference Type: string, array, class and interface
        // are reference types (can be null).
        // Title can't be null and must contain maximum 50 characters.
        [Required, StringLength(50)]
        public string Title { get; set; }

        // Value Type: Types other than string, array, class and interface
        // are value types (can't be null).
        // MealType can't be null and will have the first value of the MealTypes
        // enum if no value is assigned.
        public MealTypes MealType { get; set; }

        // Value Type:
        // MealDate can't be null and will have the default value
        // 01/01/0001 00:00:00 (DateTime.MinValue) if no value is assigned.
        public DateTime MealDate { get; set; }

        // Reference Type:
        // Notes can be null and must contain maximum 200 characters.
        // Will be null if no assignment.
        [StringLength(200)]
        public string Notes { get; set; }

        // Reference Type and Navigation Property:
        // Navigation properties are used to navigate from one entity
        // to another related entity to get or set related data.
        // Will be an empty list of MealFood if no assignment
        // or no include to a query.
        // For meals-foods many to many relationship.
        public List<MealFood> MealFoods { get; set; }
            = new(); // = new List<MealFood>(); can also be written

        // This property helps to easily manage the MealFoods relational entities
        // by Meal entity's Food Id values.
        // NotMapped attribute means no column in the Meals table will be created
        // for this property.
        [NotMapped]
        public List<int> FoodIds
        {
            // Returns the Food Id values of the Meal entity.
            get => MealFoods.Select(mealFoodEntity
                => mealFoodEntity.FoodId).ToList();

            // Sets the MealFoods relational entities of the Meal entity
            // by the assigned Food Id values.
            set => MealFoods = value.Select(foodIdValue => new MealFood
            {
                FoodId = foodIdValue
            }).ToList();
        }
    }
}
