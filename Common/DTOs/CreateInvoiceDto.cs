using System;

namespace Common.DTOs;

public class CreateInvoiceDto
{
    public string InvoiceId { get; set; } = default!;
    public DateTime DateIssued { get; set; }
    public float NetAmount { get; set; }
    public float VatAmount { get; set; }
    public float TotalAmount { get; set; }
    public string Description { get; set; } = default!;
    public string CounterPartyCompanyId { get; set; } = default!;
}