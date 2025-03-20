using Ambev.DeveloperEvaluation.Application.Sales.Actions.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Actions.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Services;

public interface ISaleDataService
{
    /// <summary>
    /// Creates a new sale from a command
    /// </summary>
    Task<SaleDataResult> CreateSaleAsync(CreateSaleCommand command);

    /// <summary>
    /// Updates an existing sale with data from a command
    /// </summary>
    Task<SaleDataResult> UpdateSaleAsync(UpdateSaleCommand command, Sale existingSale);
}
