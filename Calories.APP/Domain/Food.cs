using CORE.Domain;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Domain
{
    public class Food : Record
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ServingDescription { get; set; }

        public int Calories { get; set; }

        public decimal ProteinGrams { get; set; }

        public bool IsVegan { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public List<MealFood> MealFoods { get; set; }
            = new();
    }
}
