using Application.Common;
using MediatR;

namespace Application.Category.Command
{
    public class  DeleteCategoryCommand : IRequest<Unit>
    {
        public int CategoryId { get; private set; }

        public DeleteCategoryCommand(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        public DeleteCategoryCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                 .FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (entity == null)
                throw new ArgumentException("item not found.");

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
 }
