using Calories.APP.Domain;
using Calories.APP.Features.Foods;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Categories
{
    public class CategoryQueryRequest : Record,
        IRequest<IQueryable<CategoryQueryResponse>>
    {
    }

    public class CategoryQueryResponse : Record
    {
        public string Name { get; set; }

        public string FoodNamesR { get; set; }

        public int FoodCountR { get; set; }

        public List<FoodQueryResponse> FoodsR { get; set; }
    }

    public class CategoryQueryHandler : DbService<Category>,
        IRequestHandler<CategoryQueryRequest, IQueryable<CategoryQueryResponse>>
    {
        public CategoryQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Category> DbQuery()
        {
            return base.DbQuery()
                .Include(c => c.Foods)
                .OrderBy(c => c.Name);
        }

        public Task<IQueryable<CategoryQueryResponse>> Handle(
            CategoryQueryRequest request, CancellationToken cancellationToken)
        {
            var query = DbQuery().Select(c => new CategoryQueryResponse
            {
                Id = c.Id,
                Name = c.Name,
                FoodNamesR = string.Join(", ", c.Foods.Select(f => f.Name)),
                FoodCountR = c.Foods.Count,
                FoodsR = c.Foods.Select(f => new FoodQueryResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    ServingDescription = f.ServingDescription,
                    Calories = f.Calories,
                    ProteinGrams = f.ProteinGrams,
                    IsVegan = f.IsVegan,
                    CreatedDate = f.CreatedDate,
                    CategoryId = f.CategoryId
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
