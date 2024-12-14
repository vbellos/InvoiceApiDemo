using Common.DTOs;
using MediatR;

namespace Application.Commands;

public class CreateInvoiceCommand : IRequest<Result<InvoiceDto>>
{
    public string AuthenticatedCompanyId { get; set; } = default!;
    public CreateInvoiceDto Invoice { get; set; } = default!;
}