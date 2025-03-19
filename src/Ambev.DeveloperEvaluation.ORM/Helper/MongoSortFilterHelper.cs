using Ambev.DeveloperEvaluation.Domain.ReadModels;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Helper;

public class MongoSortFilterHelper
{
    public static FilterDefinition<SaleDocument> CreateFilter(SaleDocumentFilter filter)
    {
        var builder = Builders<SaleDocument>.Filter;
        var filters = new List<FilterDefinition<SaleDocument>>();

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

        return filters.Count > 0
            ? builder.And(filters)
            : builder.Empty;
    }

    public static SortDefinition<SaleDocument> CreateSortDefinition(string sortBy, string sortOrder)
    {
        var sortBuilder = Builders<SaleDocument>.Sort;

        bool isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        switch (sortBy.ToLower())
        {
            case "saledate":
                return isDescending
                    ? sortBuilder.Descending(x => x.SaleDate)
                    : sortBuilder.Ascending(x => x.SaleDate);

            case "customername":
                return isDescending
                    ? sortBuilder.Descending(x => x.Customer.Name)
                    : sortBuilder.Ascending(x => x.Customer.Name);

            case "branchname":
                return isDescending
                    ? sortBuilder.Descending(x => x.Branch.Name)
                    : sortBuilder.Ascending(x => x.Branch.Name);

            case "totalamount":
                return isDescending
                    ? sortBuilder.Descending(x => x.TotalAmount)
                    : sortBuilder.Ascending(x => x.TotalAmount);

            default:
                return sortBuilder.Descending(x => x.SaleDate);
        }
    }
}
