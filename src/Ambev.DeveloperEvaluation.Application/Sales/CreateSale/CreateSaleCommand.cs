using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleCommand
{
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public DateTime SaleDate { get; set; }
    public List<CreateSaleItemCommand> Items { get; set; }
}

public class CreateSaleItemCommand
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}