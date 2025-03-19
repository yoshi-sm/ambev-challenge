using Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.WebApi.Common;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sales operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    /// <param name="mediator">The mediator instance</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSales([FromQuery] GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(new ApiResponse { Success = false, Errors = response.GetErrorDetail() });
        return Ok(new PaginatedResponse<SaleDocument>
        {
            Success = true,
            Data = response.Data,
            CurrentPage = request.PageNumber,
            TotalCount = (int)response.TotalItems,
            TotalPages = (int)Math.Ceiling(response.TotalItems / (double)request.PageSize)
        });
        
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSaleQuery(id), cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(new ApiResponse { Success = false, Errors = response.GetErrorDetail() });
        return Ok(new ApiResponseWithData<SaleDocument>
        {
            Success = true,
            Data = response.Data,
        });

    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response.StatusCode == StatusCodes.Status400BadRequest)
            return BadRequest(new ApiResponse { Success = false, Errors = response.GetErrorDetail() });
        return Created(string.Empty, new ApiResponseWithData<SaleDocument>
        {
            Success = true,
            Data = response.Data
        });
    }

    [HttpPut("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSale([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CancelSaleCommand(id), cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(new ApiResponse { Success = false, Errors = response.GetErrorDetail() });
        return Ok(new ApiResponseWithData<SaleDocument>
        {
            Success = true,
            Data = response.Data
        });
    }

    [HttpPut("{id}/item/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelSaleItem([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CancelSaleItemCommand(id), cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(new ApiResponse { Success = false, Errors = response.GetErrorDetail() });
        return Ok(new ApiResponseWithData<SaleDocument>
        {
            Success = true,
            Data = response.Data
        });
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(new ApiResponse { Success = response.Success, Errors = response.GetErrorDetail() });
        if (response.StatusCode == StatusCodes.Status400BadRequest)
            return BadRequest(new ApiResponse { Success = response.Success, Errors = response.GetErrorDetail() });
        return Ok(new ApiResponseWithData<SaleDocument>
        {
            Success = response.Success,
            Data = response.Data
        });
    }
}
