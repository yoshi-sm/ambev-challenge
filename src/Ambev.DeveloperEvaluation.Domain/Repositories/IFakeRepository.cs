using Ambev.DeveloperEvaluation.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IFakeRepository
{
    CustomerInfo GetCustomerById(Guid id);
    BranchInfo GetBranchById(Guid id);
    IEnumerable<ProductInfo> GetProductsByIds(IEnumerable<Guid> ids);
}
