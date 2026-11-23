using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Foods
{
    public class FoodUpdateRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ServingDescription { get; set; }

        [Required, Range(0, 5000)]
        public int? Calories { get; set; }

        [Required, Range(0, 1000)]
        public decimal? ProteinGrams { get; set; }

        public bool IsVegan { get; set; }


        [Required]
        public int? CategoryId { get; set; }
    }

    public class FoodUpdateHandler : DbService<Food>,
        IRequestHandler<FoodUpdateRequest, CommandResponse>
    {
        public FoodUpdateHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(FoodUpdateRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery().AnyAsync(f => f.Id != request.Id &&
                f.Name == request.Name.Trim(), cancellationToken))
                return Error("Food with the same name exists!");

            var foodEntity = await DbSingleAsync(request.Id, cancellationToken);
            if (foodEntity is null)
                return Error("Food not found!");

            foodEntity.Name = request.Name.Trim();
            foodEntity.ServingDescription = request.ServingDescription?.Trim();
            foodEntity.Calories = request.Calories ?? 0;
            foodEntity.ProteinGrams = request.ProteinGrams ?? 0;
            foodEntity.IsVegan = request.IsVegan;
            foodEntity.CategoryId = request.CategoryId ?? 0;
            await DbUpdateAsync(foodEntity, cancellationToken);

            return Success(foodEntity.Id, "Food updated successfully.");
        }
    }
}
