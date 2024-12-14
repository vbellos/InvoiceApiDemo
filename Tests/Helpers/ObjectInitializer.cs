using Common.DTOs;
using Common.Models;

namespace Tests.Helpers;

public static class ObjectInitializer
{
    public static CreateInvoiceDto InitializeCreateInvoiceDto()
    {
        return new CreateInvoiceDto
        {
            InvoiceId = Guid.NewGuid().ToString(),
            DateIssued = DateTime.UtcNow,
            NetAmount = 100.0f,
            VatAmount = 20.0f,
            TotalAmount = 120.0f,
            Description = "Test Invoice",
            CounterPartyCompanyId = Guid.NewGuid().ToString()
        };
    }

    public static InvoiceDto InitializeInvoiceDto()
    {
        return new InvoiceDto
        {
            InvoiceId = Guid.NewGuid().ToString(),
            DateIssued = DateTime.UtcNow,
            NetAmount = 100.0f,
            VatAmount = 20.0f,
            TotalAmount = 120.0f,
            Description = "Test Invoice",
            CompanyId = Guid.NewGuid().ToString(),
            CounterPartyCompanyId = Guid.NewGuid().ToString()
        };
    }

    public static FilterParameters InitializeFilterParameters()
    {
        return new FilterParameters
        {
            CounterPartyCompany = "Test Counterparty",
            DateIssued = DateTime.UtcNow,
            InvoiceId = Guid.NewGuid().ToString()
        };
    }

    public static Company InitializeCompany()
    {
        return new Company
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Company",
            Users = new[] { InitializeUser() }
        };
    }

    public static AccessToken InitializeAccessToken()
    {
        return new AccessToken
        {
            Id = Guid.NewGuid().ToString(),
            CompanyId = Guid.NewGuid().ToString(),
            IsActive = true,
            Value = Guid.NewGuid().ToString()
        };
    }

    public static Invoice InitializeInvoice()
    {
        return new Invoice
        {
            Id = Guid.NewGuid().ToString(),
            InvoiceId = Guid.NewGuid().ToString(),
            DateIssued = DateTime.UtcNow,
            NetAmount = 100.0f,
            VatAmount = 20.0f,
            TotalAmount = 120.0f,
            Description = "Test Invoice",
            CompanyId = Guid.NewGuid().ToString(),
            CounterPartyCompanyId = Guid.NewGuid().ToString()
        };
    }

    public static User InitializeUser()
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test User",
            Email = "testuser@example.com"
        };
    }
}