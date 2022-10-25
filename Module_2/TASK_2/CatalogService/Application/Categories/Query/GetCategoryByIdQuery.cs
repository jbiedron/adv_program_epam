using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Query
{
    public class GetCategoryByIdQuery : IRequest<Domain.Entities.Category>
    {
        public int CategoryId { get; private set; }

        public GetCategoryByIdQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Domain.Entities.Category>
    {
        private readonly IApplicationDbContext _context;

        public GetCategoryByIdQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<Domain.Entities.Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Categories.Where(p => p.CategoryId == request.CategoryId).FirstOrDefaultAsync();
        }
    }
}
