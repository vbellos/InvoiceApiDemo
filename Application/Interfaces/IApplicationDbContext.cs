using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; }
    DbSet<Invoice> Invoices { get; }
    DbSet<AccessToken> AccessTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}