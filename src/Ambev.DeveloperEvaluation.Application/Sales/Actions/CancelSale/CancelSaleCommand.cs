using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.CancelSale;

public class CancelSaleCommand : IRequest<SaleResult<SaleDocument>>
{
    public Guid Id { get; set; }

    public CancelSaleCommand(Guid id)
    {
        Id = id;
    }
}
