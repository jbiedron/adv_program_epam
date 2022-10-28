using Application.Common;
using MediatR;

namespace Application.Products.Command
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public int ProductId { get; private set; }

        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        public DeleteProductCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products
                .FindAsync(new object[] { request.ProductId }, cancellationToken);

            if (entity == null)
                throw new ArgumentException("item not found.");

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
