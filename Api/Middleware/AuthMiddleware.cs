using Application.Interfaces;
using Common.Constants;

namespace Api.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UseAuthentication : Attribute
{
}

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            // Check if endpoint has the UseAuthentication attribute
            var requiresAuth = endpoint.Metadata.GetMetadata<UseAuthentication>() != null;
            if (requiresAuth)
            {
                // Authentication required
                var token = context.Request.Headers[AuthConstants.Authorization].FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authorization token missing.");
                    return;
                }

                var companyId = authService.ValidateToken(token);
                if (companyId == null)
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Invalid token.");
                    return;
                }

                context.Items["CompanyId"] = companyId;
            }
        }

        await _next(context);
    }
}

