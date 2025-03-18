using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleWriteRepository
{
    Task<Sale?> GetByIdAsync(Guid id);
    Task CreateAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task<Sale?> GetBySaleItemIdAsync(Guid id);
    Task UpdateSaleAsync(Sale sale, List<SaleItem> oldItems, List<SaleItem> newItems);
}
