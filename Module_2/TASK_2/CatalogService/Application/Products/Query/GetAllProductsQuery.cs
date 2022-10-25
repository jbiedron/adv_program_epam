using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Products.Query
{
    public class GetAllProductsQuery : IRequest<List<Product>>
    {
        // nothing to do...
    }

    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllProductsQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var results = await _context.Products.ToListAsync();
            return results;
        }
    }
}
