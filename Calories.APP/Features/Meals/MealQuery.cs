using Calories.APP.Domain;
using Calories.APP.Features.Foods;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Meals
{
    public class MealQueryRequest : Record,
        IRequest<IQueryable<MealQueryResponse>>
    {
    }

    public class MealQueryResponse : Record
    {
        public string Title { get; set; }
        public MealTypes MealType { get; set; }
        public DateTime MealDate { get; set; }
        public string Notes { get; set; }

        public string MealTypeR { get; set; }
        public string MealDateR { get; set; }

        public string FoodNamesR { get; set; }

        public List<FoodQueryResponse> FoodsR { get; set; }
    }

    public class MealQueryHandler : DbService<Meal>,
        IRequestHandler<MealQueryRequest, IQueryable<MealQueryResponse>>
    {
        public MealQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Meal> DbQuery()
        {
            return base.DbQuery()
                .Include(m => m.MealFoods)
                .ThenInclude(mf => mf.Food)
                .OrderByDescending(m => m.MealDate)
                .ThenBy(m => m.Title);
        }

        public Task<IQueryable<MealQueryResponse>> Handle(
            MealQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(m => new MealQueryResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    MealType = m.MealType,
                    MealDate = m.MealDate,
                    Notes = m.Notes,
                    FoodNamesR = string.Join(", ", m.MealFoods
                        .OrderBy(mf => mf.Food.Name)
                        .Select(mf => mf.Food.Name)),
                    FoodsR = m.MealFoods.OrderBy(mf => mf.Food.Name)
                        .Select(mf => new FoodQueryResponse
                        {
                            Id = mf.Food.Id,
                            Name = mf.Food.Name,
                            ServingDescription = mf.Food.ServingDescription,
                            Calories = mf.Food.Calories,
                            ProteinGrams = mf.Food.ProteinGrams,
                            IsVegan = mf.Food.IsVegan,
                            CreatedDate = mf.Food.CreatedDate,
                            CategoryId = mf.Food.CategoryId
                        }).ToList(),
                    MealDateR = m.MealDate.ToString("MM/dd/yyyy HH:mm:ss"),
                    MealTypeR = m.MealType.ToString()
                }));
        }
    }
}
