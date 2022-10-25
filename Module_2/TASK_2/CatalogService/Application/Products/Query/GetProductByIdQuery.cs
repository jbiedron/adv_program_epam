using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;


namespace Application.Products.Query
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int ProductId { get; private set; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IApplicationDbContext _context;

        public GetProductByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products.Where(p => p.ProductId == request.ProductId).FirstOrDefaultAsync();
        }
    }
}
