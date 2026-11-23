using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Meals
{
    public class MealAddRequest : Record, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Title { get; set; }

        public MealTypes MealType { get; set; }

        /*
        The type changed from DateTime to DateTime? to be able to use [Required]
        attribute. If the type is DateTime, the default value will be
        DateTime.MinValue and the validation will always be successful even if
        no value is assigned.
        */
        [Required(ErrorMessage = "{0} is required!")]
        public DateTime? MealDate { get; set; }

        [StringLength(200, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Notes { get; set; }

        [Required(ErrorMessage = "At least one food is required!")]
        public List<int> FoodIds { get; set; }
    }

    public class MealAddHandler : DbService<Meal>,
        IRequestHandler<MealAddRequest, CommandResponse>
    {
        public MealAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(
            MealAddRequest request, CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m =>
                m.Title == request.Title.Trim(), cancellationToken))
                return Error("Meal with the same title exists!");

            var mealEntity = new Meal
            {
                Title = request.Title.Trim(),
                MealType = request.MealType,
                MealDate = request.MealDate.Value,
                Notes = request.Notes?.Trim(),
                FoodIds = request.FoodIds
            };
            await DbAddAsync(mealEntity, cancellationToken);

            return Success(mealEntity.Id, "Meal created successfully.");
        }
    }
}
