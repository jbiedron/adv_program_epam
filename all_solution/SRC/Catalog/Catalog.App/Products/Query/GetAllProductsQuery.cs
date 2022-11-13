using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Catalog.App.Products.Dto;

namespace Application.Products.Query
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>
    {
        // nothing to do...
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProductsQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var prods = await _context.Products.ToListAsync();
            return prods.Select(x => new ProductDto(x)).ToList();
        }
    }
}
