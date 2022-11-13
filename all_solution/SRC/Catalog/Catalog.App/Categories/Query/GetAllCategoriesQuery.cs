using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Catalog.App.Categories.Dto;

namespace Application.Categories.Query
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDto>>
    {
        // nothing to do...
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllCategoriesQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(c => new CategoryDto(c)).ToList();
        }
    }
}
