using Application.Commands;
using Application.Handlers;
using Common.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Tests.Helpers;
// Assuming your context is here

namespace Tests.Application.Handlers;

public class CreateInvoiceCommandHandlerTests
{
    private readonly ApplicationDbContext _context;
    private readonly CreateInvoiceCommandHandler _handler;

    public CreateInvoiceCommandHandlerTests()
    {
        // Create a new in-memory database for each test run
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _handler = new CreateInvoiceCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_WithUniqueInvoiceId_ReturnsSuccess()
    {
        // Arrange
        var companyId = Guid.NewGuid().ToString();
        var dto = ObjectInitializer.InitializeCreateInvoiceDto();
        dto.InvoiceId = "INV-UNIQUE";

        var command = new CreateInvoiceCommand
        {
            AuthenticatedCompanyId = companyId,
            Invoice = dto
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(dto.InvoiceId, result.Value.InvoiceId);

        // Verify it's actually saved in the DB
        var savedInvoice =
            await _context.Invoices.FirstOrDefaultAsync(i => i.InvoiceId == dto.InvoiceId && i.CompanyId == companyId);
        Assert.NotNull(savedInvoice);
    }

    [Fact]
    public async Task Handle_WithExistingInvoiceId_ReturnsFailure()
    {
        // Arrange
        var companyId = Guid.NewGuid().ToString();
        var existingInvoiceId = "INV-EXISTING";

        // Insert existing invoice
        var existingInvoice = ObjectInitializer.InitializeInvoice();
        existingInvoice.InvoiceId = existingInvoiceId;
        existingInvoice.CompanyId = companyId;
        
        _context.Invoices.Add(existingInvoice);
        await _context.SaveChangesAsync();

        var dto = ObjectInitializer.InitializeCreateInvoiceDto();
        dto.InvoiceId = existingInvoiceId;

        var command = new CreateInvoiceCommand
        {
            AuthenticatedCompanyId = companyId,
            Invoice = dto
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Contains("already exists", result.Error!);
    }
}