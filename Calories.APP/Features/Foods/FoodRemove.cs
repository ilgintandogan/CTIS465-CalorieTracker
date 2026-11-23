using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Foods
{
    public class FoodRemoveRequest : Record, IRequest<CommandResponse>
    {
    }

    public class FoodRemoveHandler : DbService<Food>,
        IRequestHandler<FoodRemoveRequest, CommandResponse>
    {
        public FoodRemoveHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(FoodRemoveRequest request,
            CancellationToken cancellationToken)
        {
            var foodEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (foodEntity is null)
                return Error("Food not found!");

            await DbRemoveAsync(foodEntity, cancellationToken);

            return Success(foodEntity.Id, "Food deleted successfully.");
        }
    }
}
