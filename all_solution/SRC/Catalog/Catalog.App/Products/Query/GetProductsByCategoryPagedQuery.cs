using Application.Common;
using Catalog.App.Common;
using Catalog.App.Products.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.App.Products.Query
{
    public class GetProductsByCategoryPagedQuery : PagedQueryRequest, IRequest<PagedResponse<List<ProductDto>>>
    {
        public GetProductsByCategoryPagedQuery(int categoryId, int pageNo, int pageSize) : base(pageNo, pageSize)
        {
            this.CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductsByCategoryPagedQuery, PagedResponse<List<ProductDto>>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<PagedResponse<List<ProductDto>>> Handle(GetProductsByCategoryPagedQuery request, CancellationToken cancellationToken)
        {

            var pagedData = await this._context.Products
                .Where(p => p.Category.CategoryId == request.CategoryId)
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = await this._context.Products
                .Where(p => p.Category.CategoryId == request.CategoryId)
                .CountAsync();

            var results = new PagedResponse<List<ProductDto>>(pagedData.Select(p => new ProductDto(p)).ToList(), totalCount, request.PageNo, request.PageSize);
            return results;
        }
    }
}
