using Application.Handlers;
using Application.Interfaces;
using Application.Queries;
using Common.DTOs;
using Common.Models;
using Moq;
using Tests.Helpers;

namespace Tests.Application.Handlers;

public class GetReceivedInvoicesQueryHandlerTests
{
    private readonly Mock<IApplicationDbContext> _contextMock;
    private readonly GetReceivedInvoicesQueryHandler _handler;

    public GetReceivedInvoicesQueryHandlerTests()
    {
        _contextMock = new Mock<IApplicationDbContext>();
        _handler = new GetReceivedInvoicesQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsFilteredInvoices()
    {
        // Arrange
        var invoice1 = ObjectInitializer.InitializeInvoice();
        invoice1.CounterPartyCompanyId = "CompanyId1";
        var invoice2 = ObjectInitializer.InitializeInvoice();
        invoice2.CounterPartyCompanyId = "CompanyId2";

        var invoices = new List<Invoice> { invoice1, invoice2 }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(invoices);
        _contextMock.Setup(c => c.Invoices).Returns(mockDbSet.Object);

        var query = new GetReceivedInvoicesQuery
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
        invoice1.CounterPartyCompanyId = "CompanyId1";
        invoice1.CompanyId = "CompanyA";

        var invoice2 = ObjectInitializer.InitializeInvoice();
        invoice2.CounterPartyCompanyId = "CompanyId1";
        invoice2.CompanyId = "CompanyB";

        var invoices = new List<Invoice> { invoice1, invoice2 }.AsQueryable();

        var mockDbSet = DbSetMockHelper.CreateMockDbSet(invoices);
        _contextMock.Setup(c => c.Invoices).Returns(mockDbSet.Object);

        var query = new GetReceivedInvoicesQuery
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

        var query = new GetReceivedInvoicesQuery
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