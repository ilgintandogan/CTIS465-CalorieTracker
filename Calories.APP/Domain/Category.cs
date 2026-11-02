using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Domain
{
    // Category is a Record.
    public class Category : Record
    {
        // Reference Type: string, array, class and interface
        // are reference types (can be null).
        // Name can't be null and must contain maximum 50 characters.
        [Required, StringLength(50)]
        public string Name { get; set; }

        // Reference Type and Navigation Property:
        // Navigation properties are used to navigate from one entity
        // to another related entity to get or set related data.
        // Will be an empty list of Food if no assignment
        // or no include to a query.
        // For category-foods one to many relationship.
        public List<Food> Foods { get; set; }
            = new(); // = new List<Food>(); can also be written
    }
}
