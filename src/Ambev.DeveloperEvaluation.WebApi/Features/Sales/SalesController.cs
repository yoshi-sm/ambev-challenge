using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
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
    public async Task<IActionResult> CreateSale([FromQuery] GetAllSalesQuery request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response.StatusCode == StatusCodes.Status404NotFound)
            return NotFound(response.Errors);
        return Ok(new ApiResponseWithData<object>
        {
            Success = true,
            Data = response.Data
        });
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleDocument>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        if (response.StatusCode == StatusCodes.Status400BadRequest)
            return BadRequest(response.Errors);
        return Created(string.Empty, new ApiResponseWithData<object>
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
            return NotFound(response.Errors);
        return Ok(new ApiResponseWithData<object>
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
            return NotFound(response.Errors);
        return Ok(new ApiResponseWithData<object>
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
        return Ok(new ApiResponseWithData<object>
        {
            Success = response.Success,
            Data = response.Data
        });
    }
}
