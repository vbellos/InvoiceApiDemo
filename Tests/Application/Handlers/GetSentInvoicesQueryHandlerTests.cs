using Application.Handlers;
using Application.Interfaces;
using Application.Queries;
using Common.DTOs;
using Common.Models;
using Moq;
using Tests.Helpers;

namespace Tests.Application.Handlers;

public class GetSentInvoicesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly GetSentInvoicesQueryHandler _handler;

    public GetSentInvoicesQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetSentInvoicesQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsFilteredInvoices()
    {
        // Arrange
        var invoice1 = ObjectInitializer.InitializeInvoice();
        invoice1.CompanyId = "CompanyId1";
        var invoice2 = ObjectInitializer.InitializeInvoice();
        invoice2.CompanyId = "CompanyId2";

        var invoices = new List<Invoice> { invoice1, invoice2 }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(invoices);
        _contextMock.Setup(c => c.Invoices).Returns(mockDbSet.Object);

        var query = new GetSentInvoicesQuery
        {
            CompanyId = "CompanyId1",
            Filters = new FilterParameters()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(invoice1.InvoiceId, result[0].InvoiceId);
    }

    [Fact]
    public async Task Handle_WithFilters_ReturnsFilteredInvoices()
    {
        // Arrange
        var invoice1 = ObjectInitializer.InitializeInvoice();
        invoice1.CompanyId = "CompanyId1";
        invoice1.CounterPartyCompanyId = "CompanyA";

        var invoice2 = ObjectInitializer.InitializeInvoice();
        invoice2.CompanyId = "CompanyId1";
        invoice2.CounterPartyCompanyId = "CompanyB";

        var invoices = new List<Invoice> { invoice1, invoice2 }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(invoices);
        _contextMock.Setup(c => c.Invoices).Returns(mockDbSet.Object);

        var query = new GetSentInvoicesQuery
        {
            CompanyId = "CompanyId1",
            Filters = new FilterParameters
            {
                CounterPartyCompany = "CompanyA"
            }
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(invoice1.InvoiceId, result[0].InvoiceId);
    }

    [Fact]
    public async Task Handle_NoInvoicesFound_ReturnsEmptyList()
    {
        // Arrange
        var invoices = new List<Invoice>().AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(invoices);
        _contextMock.Setup(c => c.Invoices).Returns(mockDbSet.Object);

        var query = new GetSentInvoicesQuery
        {
            CompanyId = "NonExistentCompany",
            Filters = new FilterParameters()
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}