using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;
using Catalog.App.Products.Dto;

namespace Application.Products.Query
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int ProductId { get; private set; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var prod = await _context.Products.Where(p => p.ProductId == request.ProductId).FirstOrDefaultAsync();
            return prod == null ? null : new ProductDto(prod);
        }
    }
}
