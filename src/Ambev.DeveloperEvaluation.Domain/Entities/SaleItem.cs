using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public bool IsCancelled { get; private set; }

    public SaleItem(Guid saleId, Guid productId, int quantity, decimal discount, decimal totalPrice, bool isCancelled)
    {
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        Discount = discount;
        TotalPrice = totalPrice;
        IsCancelled = isCancelled;
    }

    public SaleItem() {}
}
