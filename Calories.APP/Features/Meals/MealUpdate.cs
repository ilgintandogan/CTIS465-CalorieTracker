using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Meals
{
    public class MealUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string Title { get; set; }

        public MealTypes MealType { get; set; }

        [Required]
        public DateTime? MealDate { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        [Required]
        public List<int> FoodIds { get; set; }
    }

    public class MealUpdateHandler : DbService<Meal>,
        IRequestHandler<MealUpdateRequest, CommandResponse>
    {
        public MealUpdateHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Meal> DbQuery()
        {
            return base.DbQuery().Include(m => m.MealFoods);
        }

        public async Task<CommandResponse> Handle(MealUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(m => m.Id != request.Id &&
                m.Title == request.Title.Trim(), cancellationToken))
                return Error("Meal with the same title exists!");

            var mealEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (mealEntity is null)
                return Error("Meal not found!");

            if (request.FoodIds is not null)
                DbRemove(mealEntity.MealFoods);

            mealEntity.Title = request.Title.Trim();
            mealEntity.MealType = request.MealType;
            mealEntity.MealDate = request.MealDate.Value;
            mealEntity.Notes = request.Notes?.Trim();
            mealEntity.FoodIds = request.FoodIds;
            await DbUpdateAsync(mealEntity, cancellationToken);

            return Success(mealEntity.Id, "Meal updated successfully!");
        }
    }
}
