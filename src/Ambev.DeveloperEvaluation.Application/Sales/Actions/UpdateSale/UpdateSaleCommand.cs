using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Dto;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;

public class UpdateSaleCommand : IRequest<SaleResult<SaleDocument>>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<SaleItemDto> Items { get; set; }
}


