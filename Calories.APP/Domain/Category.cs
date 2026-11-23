using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Domain
{
    public class Category : Record
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        public List<Food> Foods { get; set; }
            = new();
    }
}
