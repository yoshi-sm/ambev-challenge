using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Helper;

public class MongoFilterHelper
{
    public static FilterDefinition<SaleDocument> CreateFilter(SaleDocumentFilter filter)
    {
        var builder = Builders<SaleDocument>.Filter;
        var filters = new List<FilterDefinition<SaleDocument>>();

        // Add filters only if the parameter has a value
        if (!string.IsNullOrWhiteSpace(filter.SaleNumber))
            filters.Add(builder.Regex(x => x.SaleNumber, new BsonRegularExpression(filter.SaleNumber, "i")));

        if (filter.SaleDateFrom.HasValue)
            filters.Add(builder.Gte(x => x.SaleDate, filter.SaleDateFrom.Value));

        if (filter.SaleDateTo.HasValue)
            filters.Add(builder.Lte(x => x.SaleDate, filter.SaleDateTo.Value));

        if (filter.CustomerId.HasValue)
            filters.Add(builder.Eq(x => x.Customer.Id, filter.CustomerId.Value));

        if (filter.BranchId.HasValue)
            filters.Add(builder.Eq(x => x.Branch.Id, filter.BranchId.Value));

        if (filter.IsCancelled.HasValue)
            filters.Add(builder.Eq(x => x.IsCancelled, filter.IsCancelled.Value));

        if (filter.MinTotalAmount.HasValue)
            filters.Add(builder.Gte(x => x.TotalAmount, filter.MinTotalAmount.Value));

        if (filter.MaxTotalAmount.HasValue)
            filters.Add(builder.Lte(x => x.TotalAmount, filter.MaxTotalAmount.Value));

        // Combine all filters with AND operator
        return filters.Count > 0
            ? builder.And(filters)
            : builder.Empty;
    }
}
