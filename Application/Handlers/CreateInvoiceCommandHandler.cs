using Application.Commands;
using Application.Interfaces;
using Common;
using Common.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Result<InvoiceDto>>
{
    private readonly IApplicationDbContext _context;

    public CreateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<InvoiceDto>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var existingInvoice = await _context.Invoices
            .Where(i => i.InvoiceId == request.Invoice.InvoiceId && i.CompanyId == request.AuthenticatedCompanyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (existingInvoice != null)
            return Result<InvoiceDto>.Failure($"Invoice with ID {request.Invoice.InvoiceId} already exists.");

        var invoice = request.Invoice.MapCreateToInvoice(request.AuthenticatedCompanyId);
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<InvoiceDto>.Success(invoice.MapToInvoiceDto());
    }
}