using Calories.APP.Domain;
using CORE.Domain;
using CORE.Models;
using CORE.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Calories.APP.Features.Categories
{
    public class CategoryAddRequest : Record, IRequest<CommandResponse>
    {
        [Required, StringLength(50)]
        public string Name { get; set; }
    }

    public class CategoryAddHandler : DbService<Category>,
        IRequestHandler<CategoryAddRequest, CommandResponse>
    {
        public CategoryAddHandler(DbContext db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(CategoryAddRequest request,
            CancellationToken cancellationToken)
        {
            if (await DbQuery()
                .AnyAsync(categoryEntity =>
                    categoryEntity.Name == request.Name.Trim(),
                        cancellationToken))
                return Error("Category with the same name exists!");

            var categoryEntity = new Category
            {
                Name = request.Name.Trim()
            };
            await DbAddAsync(categoryEntity, cancellationToken);

            return Success(categoryEntity.Id, "Category created successfully.");
        }
    }
}
