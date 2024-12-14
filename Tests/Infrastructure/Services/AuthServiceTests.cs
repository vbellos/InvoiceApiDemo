using Application.Interfaces;
using Common.Models;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Tests.Helpers;

namespace Tests.Infrastructure.Services;

public class AuthServiceTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _authService = new AuthService(_contextMock.Object);
    }
    

    [Fact]
    public void ValidateToken_ValidToken_ReturnsCompanyId()
    {
        // Arrange
        var token = "ValidToken";
        var companyId = "12345";

        var accessTokens = new List<AccessToken>
        {
            new AccessToken
            {
                Value = token,
                IsActive = true,
                CompanyId = companyId
            }
        }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(accessTokens);
        _contextMock.Setup(context => context.AccessTokens).Returns(mockDbSet.Object);

        // Act
        var result = _authService.ValidateToken(token);

        // Assert
        Assert.Equal(companyId, result);
    }

    [Fact]
    public void ValidateToken_InvalidToken_ReturnsNull()
    {
        // Arrange
        var token = "InvalidToken";

        var accessTokens = new List<AccessToken>
        {
            new AccessToken
            {
                Value = "SomeOtherToken",
                IsActive = true,
                CompanyId = "12345"
            }
        }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(accessTokens);
        _contextMock.Setup(context => context.AccessTokens).Returns(mockDbSet.Object);

        // Act
        var result = _authService.ValidateToken(token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateToken_InactiveToken_ReturnsNull()
    {
        // Arrange
        var token = "InactiveToken";

        var accessTokens = new List<AccessToken>
        {
            new AccessToken
            {
                Value = token,
                IsActive = false,
                CompanyId = "12345"
            }
        }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(accessTokens);
        _contextMock.Setup(context => context.AccessTokens).Returns(mockDbSet.Object);

        // Act
        var result = _authService.ValidateToken(token);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ValidateToken_NoMatchingToken_ReturnsNull()
    {
        // Arrange
        var token = "NonExistentToken";

        var accessTokens = new List<AccessToken>().AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(accessTokens);
        _contextMock.Setup(context => context.AccessTokens).Returns(mockDbSet.Object);

        // Act
        var result = _authService.ValidateToken(token);

        // Assert
        Assert.Null(result);
    }
}