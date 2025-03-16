using Ambev.DeveloperEvaluation.Domain.Common;
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
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public ICollection<SaleItem> Items { get; private set; }

    public Sale() {}

    public Sale(string saleNumber, DateTime saleDate, Guid customerId, Guid branchId, decimal totalAmount, bool isCancelled, ICollection<SaleItem> items)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        BranchId = branchId;
        TotalAmount = totalAmount;
        IsCancelled = isCancelled;
        Items = items;
    }

    public void Cancel()
    {
        IsCancelled = true;
        foreach(SaleItem item in Items)
        {
            item.Cancel();
        }
    }
}
