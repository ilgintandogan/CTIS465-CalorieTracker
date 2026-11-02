using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Domain
{
    // Food is a Record.
    public class Food : Record
    {
        // Reference Type: string, array, class and interface
        // are reference types (can be null).
        // Name can't be null and must contain maximum 100 characters.
        [Required, StringLength(100)]
        public string Name { get; set; }

        // Reference Type:
        // ServingDescription can be null and must contain maximum 50 characters.
        // Will be null if no assignment.
        [StringLength(50)]
        public string ServingDescription { get; set; }

        // Value Type: Types other than string, array, class and interface
        // are value types (can't be null).
        // Calories can't be null and will have the default value 0
        // if no value is assigned.
        public int Calories { get; set; }

        // Value Type:
        // ProteinGrams can't be null and will have the default value 0
        // if no value is assigned.
        public decimal ProteinGrams { get; set; }

        // Value Type:
        // IsVegan can't be null and will have the default value false
        // if no value is assigned.
        public bool IsVegan { get; set; }

        // Value Type:
        // CreatedDate can't be null and will have the default value
        // 01/01/0001 00:00:00 (DateTime.MinValue) if no value is assigned.
        public DateTime CreatedDate { get; set; }

        // Value Type:
        // CategoryId can't be null and will have the default value 0
        // if no value is assigned.
        // For foods-category one to many relationship.
        public int CategoryId { get; set; } // foreign key to the Category entity

        // Reference Type and Navigation Property:
        // Navigation properties are used to navigate from one entity
        // to another related entity to get or set related data.
        // Will be null if no assignment or no include to a query.
        // For foods-category one to many relationship.
        public Category Category { get; set; }

        // Reference Type and Navigation Property:
        // Will be an empty list of MealFood if no assignment
        // or no include to a query.
        // For foods-meals many to many relationship.
        public List<MealFood> MealFoods { get; set; }
            = new(); // = new List<MealFood>(); can also be written
    }
}
