using System;
using Common.DTOs;
using Common.Models;

namespace Common;

public static class ObjectMapper
{
    public static InvoiceDto MapToInvoiceDto(this Invoice invoice)
    {
        return new InvoiceDto
        {
            InvoiceId = invoice.InvoiceId,
            DateIssued = invoice.DateIssued,
            NetAmount = invoice.NetAmount,
            VatAmount = invoice.VatAmount,
            TotalAmount = invoice.TotalAmount,
            Description = invoice.Description,
            CompanyId = invoice.CompanyId,
            CounterPartyCompanyId = invoice.CounterPartyCompanyId
        };
    }

    public static Invoice MapCreateToInvoice(this CreateInvoiceDto invoiceDto, string companyId)
    {
        return new Invoice
        {
            Id = Guid.NewGuid().ToString(), // Use Guid.NewGuid() to generate a unique ID
            InvoiceId = invoiceDto.InvoiceId,
            DateIssued = invoiceDto.DateIssued,
            NetAmount = invoiceDto.NetAmount,
            VatAmount = invoiceDto.VatAmount,
            TotalAmount = invoiceDto.TotalAmount,
            Description = invoiceDto.Description,
            CompanyId = companyId,
            CounterPartyCompanyId = invoiceDto.CounterPartyCompanyId
        };
    }
}