using Domain.Entities;
using MediatR;
using Application.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Query
{
    public class GetAllCategoriesQuery : IRequest<List<Domain.Entities.Category>>
    {
        // nothing to do...
    }

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<Domain.Entities.Category>>
    {
        private readonly IApplicationDbContext _context;

        public GetAllCategoriesQueryHandler(IApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Domain.Entities.Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var results = await _context.Categories.ToListAsync();
            return results;
        }
    }
}
