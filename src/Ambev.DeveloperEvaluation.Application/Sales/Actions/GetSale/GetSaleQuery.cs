using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Actions.GetSale;

public class GetSaleQuery : IRequest<SaleResult<SaleDocument>>
{
    public Guid Id { get; set; }

    public GetSaleQuery(Guid id)
    {
        Id = id;
    }
}
