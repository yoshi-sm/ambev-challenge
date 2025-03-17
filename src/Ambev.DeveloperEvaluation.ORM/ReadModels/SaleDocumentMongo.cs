using Ambev.DeveloperEvaluation.Domain.ReadModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ambev.DeveloperEvaluation.ORM.ReadModels;

public class SaleDocumentMongo : SaleDocument
{
    public SaleDocumentMongo(SaleDocument document)
    {
        Id = document.Id;
        SaleNumber = document.SaleNumber;
        SaleDate = document.SaleDate;
        Customer = document.Customer;
        Branch = document.Branch;
        TotalAmount = document.TotalAmount;
        IsCancelled = document.IsCancelled;
        Items = document.Items;
    }
}

