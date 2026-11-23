using Calories.APP.Domain;
using Calories.APP.Features.Categories;
using CORE.Domain;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Calories.APP.Features.Foods
{
    public class FoodQueryRequest : Record,
        IRequest<IQueryable<FoodQueryResponse>>
    {
    }

    public class FoodQueryResponse : Record
    {
        public string Name { get; set; }
        public string ServingDescription { get; set; }
        public int Calories { get; set; }
        public decimal ProteinGrams { get; set; }
        public bool IsVegan { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }

        public string ProteinGramsR { get; set; }
        public string IsVeganR { get; set; }
        public string CreatedDateR { get; set; }

        public string CategoryNameR { get; set; }
        public CategoryQueryResponse CategoryR { get; set; }
    }

    public class FoodQueryHandler : DbService<Food>,
        IRequestHandler<FoodQueryRequest, IQueryable<FoodQueryResponse>>
    {
        public FoodQueryHandler(DbContext db) : base(db)
        {
        }

        protected override IQueryable<Food> DbQuery()
        {
            return base.DbQuery()
                .Include(f => f.Category)
                .OrderBy(f => f.Name)
                .ThenByDescending(f => f.Calories);
        }

        public Task<IQueryable<FoodQueryResponse>> Handle(
            FoodQueryRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                DbQuery().Select(f => new FoodQueryResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    ServingDescription = f.ServingDescription,
                    Calories = f.Calories,
                    ProteinGrams = f.ProteinGrams,
                    IsVegan = f.IsVegan,
                    CreatedDate = f.CreatedDate,
                    CategoryId = f.CategoryId,
                    CategoryNameR = f.Category.Name,
                    CategoryR = new CategoryQueryResponse
                    {
                        Id = f.Category.Id,
                        Name = f.Category.Name
                    },
                    CreatedDateR = f.CreatedDate.ToString("MM/dd/yyyy HH:mm:ss"),
                    IsVeganR = f.IsVegan ? "Yes" : "No",
                    ProteinGramsR = f.ProteinGrams.ToString("N1")
                }));
        }
    }
}
