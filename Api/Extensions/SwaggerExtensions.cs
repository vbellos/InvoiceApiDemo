using NSwag;
using NSwag.Generation.Processors.Security;

namespace SoftOneAssessment.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomOpenApiDocument(this IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.Title = "Invoicing API";
            config.Description = "API for managing invoices and companies.";

            config.AddSecurity("Authorization", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Example Tokens: SoftOne : token_softone , edrink : token_edrink"
            });
            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Authorization"));
        });

        return services;
    }
}