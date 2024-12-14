using Application.Interfaces;
using Application.Queries;
using Common;
using Common.DTOs;
using Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers;

public class GetSentInvoicesQueryHandler : IRequestHandler<GetSentInvoicesQuery, List<InvoiceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSentInvoicesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<InvoiceDto>> Handle(GetSentInvoicesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Invoice> query = _context.Invoices.Where(i => i.CompanyId == request.CompanyId);

        if (!string.IsNullOrEmpty(request.Filters.CounterPartyCompany))
            query = query.Where(i => i.CounterPartyCompanyId == request.Filters.CounterPartyCompany);

        if (request.Filters.DateIssued.HasValue)
            query = query.Where(i => i.DateIssued.Date == request.Filters.DateIssued.Value.Date);

        if (!string.IsNullOrEmpty(request.Filters.InvoiceId))
            query = query.Where(i => i.InvoiceId == request.Filters.InvoiceId);

        var invoices = await query.ToListAsync(cancellationToken);

        return invoices.Select(i => i.MapToInvoiceDto()).ToList();
    }
}