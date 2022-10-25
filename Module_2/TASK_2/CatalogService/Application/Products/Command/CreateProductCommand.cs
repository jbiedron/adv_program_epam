using Application.Common;
using MediatR;

namespace Application.Products.Command
{
    public class CreateProductCommand : IRequest<int>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public uint Amount { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context) 
        {
            this._context = context;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Product()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Amount = request.Amount,
                Image = request.ImageUrl
            };

            // get category
            var category = this._context.Categories.Where(c => c.CategoryId == request.CategoryId).FirstOrDefault();
            if (category == null)
                throw new ArgumentException("category not found.");

            entity.Category = category;

            _context.Products.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.ProductId;
        }
    }
}
