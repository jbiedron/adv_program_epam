using Application.Common;
using Catalog.App.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.App.Products.Query
{
    public class GetProductsByCategoryPagedQuery : PagedQueryRequest, IRequest<PagedResponse<List<Product>>>
    {
        public GetProductsByCategoryPagedQuery(int categoryId, int pageNo, int pageSize) : base(pageNo, pageSize)
        {
            this.CategoryId = categoryId;
        }

        public int CategoryId { get; private set; }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductsByCategoryPagedQuery, PagedResponse<List<Product>>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<PagedResponse<List<Product>>> Handle(GetProductsByCategoryPagedQuery request, CancellationToken cancellationToken)
        {

            var pagedData = await this._context.Products
                .Where(p => p.Category.CategoryId == request.CategoryId)
                .Skip((request.PageNo - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = await this._context.Products
                .Where(p => p.Category.CategoryId == request.CategoryId)
                .CountAsync();

            var results = new PagedResponse<List<Product>>(pagedData, totalCount, request.PageNo, request.PageSize);
            return results;
        }
    }
}
