using Application.Interfaces;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<AccessToken> AccessTokens => Set<AccessToken>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => base.SaveChangesAsync(cancellationToken);
}