using Api.Middleware;
using Application.Interfaces;
using Common.Constants;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests.Api.Middleware;
 
 public class AuthMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthMiddleware _authMiddleware;

        public AuthMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _authServiceMock = new Mock<IAuthService>();
            _authMiddleware = new AuthMiddleware(_nextMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_EndpointWithoutUseAuthenticationAttribute_CallsNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var endpoint = new Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(), "TestEndpoint");
            context.SetEndpoint(endpoint);

            // Act
            await _authMiddleware.InvokeAsync(context, _authServiceMock.Object);

            // Assert
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_NoAuthorizationHeader_Returns401()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var endpoint = new Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(new UseAuthentication()), "TestEndpoint");
            context.SetEndpoint(endpoint);

            // Act
            await _authMiddleware.InvokeAsync(context, _authServiceMock.Object);

            // Assert
            Assert.Equal(401, context.Response.StatusCode);
            _nextMock.Verify(next => next(context), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_InvalidToken_Returns403()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Headers[AuthConstants.Authorization] = "InvalidToken";

            var endpoint = new Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(new UseAuthentication()), "TestEndpoint");
            context.SetEndpoint(endpoint);

            _authServiceMock.Setup(authService => authService.ValidateToken(It.IsAny<string>())).Returns((string)null);

            // Act
            await _authMiddleware.InvokeAsync(context, _authServiceMock.Object);

            // Assert
            Assert.Equal(403, context.Response.StatusCode);
            _nextMock.Verify(next => next(context), Times.Never);
        }

        [Fact]
        public async Task InvokeAsync_ValidToken_SetsCompanyIdAndCallsNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var token = "ValidToken";
            var companyId = "12345";

            context.Request.Headers[AuthConstants.Authorization] = token;
            var endpoint = new Endpoint(_ => Task.CompletedTask, new EndpointMetadataCollection(new UseAuthentication()), "TestEndpoint");
            context.SetEndpoint(endpoint);

            _authServiceMock.Setup(authService => authService.ValidateToken(token)).Returns(companyId);

            // Act
            await _authMiddleware.InvokeAsync(context, _authServiceMock.Object);

            // Assert
            Assert.True(context.Items.ContainsKey("CompanyId"));
            Assert.Equal(companyId, context.Items["CompanyId"]);
            _nextMock.Verify(next => next(context), Times.Once);
        }
    }