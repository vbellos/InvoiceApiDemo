using Application.Queries;
using Common.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationModule(this IServiceCollection services)
    {
        // Register MediatR for both Application and Common.DTOs assemblies
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(
                typeof(InvoiceDto).Assembly, // Common.DTOs
                typeof(GetReceivedInvoicesQuery).Assembly // Application
            ));

        return services;
    }
}