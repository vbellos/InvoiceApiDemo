using Api.Middleware;
using Application.Commands;
using Application.Queries;
using Common.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoftOneAssessment.Extensions;

namespace Api.Controllers;

/// <summary>
///     API for managing invoices.
/// </summary>
[ApiController]
[Route("invoice")]
[UseAuthentication] // Use Authorization Middleware
public class InvoiceController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Creates a new invoice.
    /// </summary>
    /// <param name="dto">Details of the invoice to create.</param>
    /// <returns>The created invoice with its ID.</returns>
    /// <remarks>
    ///     Authorization token is required in the request header.
    /// 
    ///     Example: `Authorization: token_softone`
    /// 
    ///     Sample JSON payload:
    /// 
    ///     `{
    ///     "invoiceId": "INV001",
    ///     "dateIssued": "2024-01-01T00:00:00Z",
    ///     "netAmount": 100.0,
    ///     "vatAmount": 20.0,
    ///     "totalAmount": 120.0,
    ///     "description": "Invoice for services rendered.",
    ///     "counterPartyCompanyId": "softone"
    ///     }`
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
    {
        var command = new CreateInvoiceCommand
        {
            AuthenticatedCompanyId = HttpContext.GetCompanyId()!,
            Invoice = dto
        };

        var result = await _mediator.Send(command);
        if (!result.IsSuccess)
            // Return 409 Conflict if invoice already exists
            return Conflict(new { error = result.Error });

        return Created($"/invoice/{result.Value!.InvoiceId}", result.Value);
    }

    /// <summary>
    ///     Retrieves invoices sent by the authenticated company.
    /// </summary>
    /// <param name="counter_party_company">Filter by counter-party company ID.</param>
    /// <param name="date_issued">Filter by the date the invoice was issued.</param>
    /// <param name="invoice_id">Filter by the invoice ID.</param>
    /// <returns>A list of sent invoices.</returns>
    /// <remarks>
    ///     Authorization token is required in the request header.
    /// 
    ///     Example: `Authorization: token_softone`
    /// 
    ///     Filters can be applied using query parameters:
    /// 
    ///     - `counter_party_company` (optional): ID of the counter-party company.
    /// 
    ///     - `date_issued` (optional): Date when the invoice was issued.
    /// 
    ///     - `invoice_id` (optional): ID of the specific invoice.
    /// </remarks>
    [HttpGet("sent")]
    public async Task<IActionResult> GetSentInvoices(
        [FromQuery] string? counter_party_company,
        [FromQuery] DateTime? date_issued,
        [FromQuery] string? invoice_id)
    {
        var query = new GetSentInvoicesQuery
        {
            CompanyId = HttpContext.GetCompanyId()!,
            Filters = new FilterParameters
            {
                CounterPartyCompany = counter_party_company,
                DateIssued = date_issued,
                InvoiceId = invoice_id
            }
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    ///     Retrieves invoices received by the authenticated company.
    /// </summary>
    /// <param name="counter_party_company">Filter by sender company ID.</param>
    /// <param name="date_issued">Filter by the date the invoice was issued.</param>
    /// <param name="invoice_id">Filter by the invoice ID.</param>
    /// <returns>A list of received invoices.</returns>
    /// <remarks>
    ///     Authorization token is required in the request header.
    /// 
    ///     Example: `Authorization: token_edrink`
    /// 
    ///     Filters can be applied using query parameters:
    /// 
    ///     - `counter_party_company` (optional): ID of the sender company.
    /// 
    ///     - `date_issued` (optional): Date when the invoice was issued.
    /// 
    ///     - `invoice_id` (optional): ID of the specific invoice.
    /// </remarks>
    [HttpGet("received")]
    public async Task<IActionResult> GetReceivedInvoices(
        [FromQuery] string? counter_party_company,
        [FromQuery] DateTime? date_issued,
        [FromQuery] string? invoice_id)
    {
        var query = new GetReceivedInvoicesQuery
        {
            CompanyId = HttpContext.GetCompanyId()!,
            Filters = new FilterParameters
            {
                CounterPartyCompany = counter_party_company,
                DateIssued = date_issued,
                InvoiceId = invoice_id
            }
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}