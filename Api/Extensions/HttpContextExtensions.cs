using Common.Constants;
using Common.Models;

namespace SoftOneAssessment.Extensions;

public static class HttpContextExtensions
{
    public static string? GetCompanyId(this HttpContext context)
    {
        return context.Items[AuthConstants.CompanyId] as string;
    }
}