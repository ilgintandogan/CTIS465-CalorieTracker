using CORE.Domain;

namespace Calories.APP.Domain
{
    // MealFood is a Record.
    // For meals-foods many to many relationship.
    public class MealFood : Record
    {
        // Value Type: Types other than string, array, class and interface
        // are value types (can't be null).
        // MealId can't be null and will have the default value 0
        // if no value is assigned.
        public int MealId { get; set; } // foreign key to the Meal entity

        // Reference Type and Navigation Property: string, array, class
        // and interface are reference types (can be null).
        // Navigation properties are used to navigate from one entity
        // to another related entity to get or set related data.
        // Will be null if no assignment or no include to a query.
        public Meal Meal { get; set; }

        // Value Type:
        // FoodId can't be null and will have the default value 0
        // if no value is assigned.
        public int FoodId { get; set; } // foreign key to the Food entity

        // Reference Type and Navigation Property:
        // Will be null if no assignment or no include to a query.
        public Food Food { get; set; }
    }
}
