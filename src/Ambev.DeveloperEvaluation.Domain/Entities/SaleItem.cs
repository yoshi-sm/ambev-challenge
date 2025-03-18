using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.Results;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid SaleId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice { get; private set; }
    public bool IsCancelled { get; private set; }

    public SaleItem() {}

    public SaleItem(Guid saleId, Guid productId, string productName, decimal unitPrice, 
        int quantity, decimal discount, decimal totalPrice, bool isCancelled)
    {
        SaleId = saleId;
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        Discount = discount;
        TotalPrice = totalPrice;
        IsCancelled = isCancelled;
    }

    public ValidationResult Validate()
    {
        var validator = new SaleItemValidator();
        return validator.Validate(this);
    }

    public void Cancel()
    {
        IsCancelled = true;
    }

    public void SetSaleItem(Guid saleId)
    {
        SaleId = saleId;
        GetDiscountPercentage();
        CalculatePrices();
    }

    private void CalculatePrices()
    {
        decimal subtotal = UnitPrice * Quantity;
        decimal discountValue = subtotal * Discount;

        TotalPrice = subtotal - discountValue;
    }

    private void GetDiscountPercentage()
    {
        if (Quantity >= 10 && Quantity <= 20)
            Discount = 0.20m;
        else if (Quantity >= 4)
            Discount = 0.10m; 
        else
            Discount = 0;
    }
}
