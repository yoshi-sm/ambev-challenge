using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public bool IsCancelled { get; private set; }

    public SaleItem(Guid saleId, Product product, int quantity, decimal unitPrice, decimal discount, decimal totalPrice, bool isCancelled)
    {
        SaleId = saleId;
        Product = product;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        TotalPrice = totalPrice;
        IsCancelled = isCancelled;
    }

    public SaleItem() {}
}
