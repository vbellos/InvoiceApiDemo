namespace Application.Interfaces;

public interface IAuthService
{
    string? ValidateToken(string token);
}