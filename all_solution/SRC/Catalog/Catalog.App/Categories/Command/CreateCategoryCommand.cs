using Application.Common;
using MediatR;

namespace Application.Category.Command
{
    public class CreateCategoryCommand : IRequest<int>
    {
        public string Name { get; set; }                // required, plain text, max-length=50
        public string Image { get; set; }               // optional, url
        public int? ParentId { get; set; }              // optional
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCategoryCommandHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.Category()
            {
                Name = request.Name,
                Image = request.Image
            };

            // get category
            if (request.ParentId.HasValue)
            {
                var parent = await _context.Categories
                    .FindAsync(new object[] { request.ParentId.Value }, cancellationToken);

                if (parent == null)
                    throw new ArgumentException("parent category not found.");

                entity.Parent = parent;
            }

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.CategoryId;
        }
    }
}
