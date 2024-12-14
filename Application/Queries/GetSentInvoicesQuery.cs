using Common.DTOs;
using MediatR;

namespace Application.Queries;

public class GetSentInvoicesQuery : IRequest<List<InvoiceDto>>
{
    public string CompanyId { get; set; } = default!;
    public FilterParameters Filters { get; set; } = new();
}