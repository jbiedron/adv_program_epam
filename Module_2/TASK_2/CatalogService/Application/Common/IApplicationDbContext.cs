

using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Application.Common
{
    // this interface can also support start/end transaction method
    public interface IApplicationDbContext
    {
        DbSet<Domain.Entities.Category> Categories { get; }

        DbSet<Product> Products { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
