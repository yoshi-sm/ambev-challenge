using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public Guid BranchId { get; private set; }
    public string BranchName { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; } = false;
    public ICollection<SaleItem> Items { get; private set; }

    public Sale() {}

    public Sale(string saleNumber, DateTime saleDate, Guid customerId, string customerName, 
        Guid branchId, string branchName, decimal totalAmount, 
        bool isCancelled, ICollection<SaleItem> items)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        TotalAmount = totalAmount;
        IsCancelled = isCancelled;
        Items = items;
    }

    public ValidationResult Validate()
    {
        var validator = new SaleValidator();
        return validator.Validate(this);
    }

    public void Cancel()
    {
        IsCancelled = true;
        if(Items != null)
        {
            foreach (SaleItem item in Items)
            {
                item.Cancel();
            }
        }
    }
    public ValidationResult SetSale(ICollection<SaleItem> items)
    {
        Items = items;
        GenerateSaleNumber();
        CalculateTotalAmount();
        SaleDate = DateTime.Now;
        return Validate();
    }

    private void GenerateSaleNumber()
    {
        string datePart = DateTime.UtcNow.ToString("yyyyMMdd");
        string randomPart = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        SaleNumber = $"SALE-{datePart}-{randomPart}";
    }

    public void CalculateTotalAmount()
    {
        var total = 0m;
        foreach (var item in Items.Where(x => !x.IsCancelled))
        {
            item.SetSaleItem(Id);
            total += item.TotalPrice;
        }
        TotalAmount = total;
        if (total == 0)
            Cancel();
    }
}
