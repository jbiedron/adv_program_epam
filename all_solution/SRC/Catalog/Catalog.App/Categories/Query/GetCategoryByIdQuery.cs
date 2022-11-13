using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Catalog.App.Categories.Dto;

namespace Application.Categories.Query
{
    public class GetCategoryByIdQuery : IRequest<CategoryDto>
    {
        public int CategoryId { get; private set; }

        public GetCategoryByIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var cat = await _context.Categories.Where(p => p.CategoryId == request.CategoryId).FirstOrDefaultAsync();
            return cat == null ? null : new CategoryDto(cat);
        }
    }
}
