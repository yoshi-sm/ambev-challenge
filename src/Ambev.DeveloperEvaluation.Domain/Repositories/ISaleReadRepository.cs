using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleReadRepository
{
    Task<(IEnumerable<SaleDocument>, long)> GetAllAsync(SaleDocumentFilter filter);
    Task<SaleDocument?> GetByIdAsync(Guid id);
    Task ReplaceAsync(SaleDocument entity);
    Task InsertAsync(SaleDocument entity);
    Task<SaleDocument?> GetSaleByItemIdAsync(Guid id);
}
