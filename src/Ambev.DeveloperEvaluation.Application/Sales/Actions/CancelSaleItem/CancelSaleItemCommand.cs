using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSaleItem;

public class CancelSaleItemCommand : IRequest<SaleResult<SaleDocument>>
{
    public Guid ItemId { get; set; }

    public CancelSaleItemCommand(Guid itemId)
    {
        ItemId = itemId;
    }
}
