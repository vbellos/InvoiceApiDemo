using System;

namespace Common.DTOs;

public class FilterParameters
{
    public string? CounterPartyCompany { get; set; }
    public DateTime? DateIssued { get; set; }
    public string? InvoiceId { get; set; }
}