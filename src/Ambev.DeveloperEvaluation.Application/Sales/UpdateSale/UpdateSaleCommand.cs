using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

public class UpdateSaleCommand : IRequest<SaleResult>
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public List<CreateSaleItemCommand> Items { get; set; }
}

public class UpdateSaleItemCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
