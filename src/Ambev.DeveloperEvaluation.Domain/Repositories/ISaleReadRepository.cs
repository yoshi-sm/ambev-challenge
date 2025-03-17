using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ISaleReadRepository
{
    Task<IEnumerable<SaleDocument>> GetAllAsync();
    Task<SaleDocument?> GetByIdAsync(Guid id);
    Task ReplaceAsync(SaleDocument entity);
    Task InsertAsync(SaleDocument entity);
}
