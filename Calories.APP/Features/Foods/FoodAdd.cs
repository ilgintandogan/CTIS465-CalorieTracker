using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Foods
{
    public class FoodAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string ServingDescription { get; set; }

        /*
        The type changed from int to int? to be able to use [Required]
        attribute. If the type is int, the default value will be 0 and
        the validation will always be successful even if no value is assigned.
        */
        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 5000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public int? Calories { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [Range(0, 1000, ErrorMessage = "{0} must be between {1} and {2}!")]
        public decimal? ProteinGrams { get; set; }

        public bool IsVegan { get; set; }


        [Required(ErrorMessage = "{0} is required!")]
        public int? CategoryId { get; set; }

    }

    public class FoodAddHandler : DbService<Food>,
        IRequestHandler<FoodAddRequest, CommandResponse>
    {
        public FoodAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(
            FoodAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(f =>
                f.Name == request.Name.Trim(), cancellationToken))
                return Error("Food with the same name exists!");

            var foodEntity = new Food
            {
                Name = request.Name.Trim(),
                ServingDescription = request.ServingDescription?.Trim(),
                Calories = request.Calories ?? 0,
                ProteinGrams = request.ProteinGrams ?? 0,
                IsVegan = request.IsVegan,
                CreatedDate = DateTime.Now,
                CategoryId = request.CategoryId ?? 0
            };
            await DbAddAsync(foodEntity, cancellationToken);

            return Success(foodEntity.Id, "Food created successfully.");
        }
    }
}
