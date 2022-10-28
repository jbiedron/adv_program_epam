using Application.Common;
using MediatR;

namespace Application.Products.Command
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public uint Amount { get; set; }
    }

    
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FindAsync(new object[] { request.ProductId }, cancellationToken);

            if (entity == null)
                throw new ArgumentException("product not found.");

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Image = request.ImageUrl;
            entity.Price = request.Price;
            entity.Amount = request.Amount;

            var category = await _context.Categories
                .FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (category == null)
                throw new ArgumentException("category not found.");

            entity.Category = category;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
