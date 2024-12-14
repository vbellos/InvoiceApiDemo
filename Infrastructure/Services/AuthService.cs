using Application.Interfaces;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _context;

    public AuthService(IApplicationDbContext context)
    {
        _context = context;
    }

    public string? ValidateToken(string token)
    {
        var tokenEntry = _context.AccessTokens
            .FirstOrDefault(t => t.Value == token && t.IsActive);

        return tokenEntry?.CompanyId;
    }
}