using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.Dto;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;

public class CreateSaleCommand : IRequest<SaleResult<SaleDocument>>
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<SaleItemDto> Items { get; set; }
}
