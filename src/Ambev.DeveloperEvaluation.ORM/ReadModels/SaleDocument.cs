using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.ReadModels;

public class SaleDocument
{
    [BsonId]
    public Guid Id { get; set; }
    public string SaleNumber { get; set; }
    public DateTime SaleDate { get; set; }
    public CustomerInfo Customer { get; set; }
    public BranchInfo Branch { get; set; }
    public decimal TotalAmount { get; set; }
    public bool IsCancelled { get; set; }
    public List<SaleItemDocument> Items { get; set; }
}

public class CustomerInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class BranchInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
