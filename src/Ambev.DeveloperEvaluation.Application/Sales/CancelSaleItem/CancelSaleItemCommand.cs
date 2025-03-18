using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

public class CancelSaleItemCommand : IRequest<SaleResult>
{
    public Guid ItemId { get; set; }

    public CancelSaleItemCommand(Guid itemId)
    {
        ItemId = itemId;
    }
}
