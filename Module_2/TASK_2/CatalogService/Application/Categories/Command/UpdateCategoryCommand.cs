using Application.Common;
using MediatR;

namespace Application.Category.Command
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }                // required, plain text, max-length=50
        public string Image { get; set; }               // optional, url
        public int? ParentId { get; set; }              // optional
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCategoryCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
               .FindAsync(new object[] { request.CategoryId }, cancellationToken);

            if (entity == null)
                throw new ArgumentException("category not found.");

            entity.Name = request.Name;
            entity.Image = request.Image;

            if (request.ParentId.HasValue)
            {
                var parent = await _context.Categories
                    .FindAsync(new object[] { request.ParentId.Value }, cancellationToken);

                if (parent == null)
                    throw new ArgumentException("parent category not found.");

                if (parent.CategoryId == request.CategoryId)
                    throw new ArgumentException("cannot self-reference");

                entity.Parent = parent;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}