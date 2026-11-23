using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Meals
{
    public class MealRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class MealRemoveHandler : DbService<Meal>,
        IRequestHandler<MealRemoveRequest, CommandResponse>
    {
        public MealRemoveHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Meal> DbQuery()
        {
            return base.DbQuery().Include(m => m.MealFoods);
        }

        public async Task<CommandResponse> Handle(MealRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var mealEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (mealEntity is null)
                return Error("Meal not found!");

            DbRemove(mealEntity.MealFoods);

            await DbRemoveAsync(mealEntity, cancellationToken);

            return Success(mealEntity.Id, "Meal deleted successfully.");
        }
    }
}
