using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
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
    public Customer Customer { get; private set; }
    public Branch Branch { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsCancelled { get; private set; }
    public ICollection<SaleItem> Items { get; private set; }

    public Sale() {}

    public Sale(string saleNumber, DateTime saleDate, Customer customer, Branch branch, decimal totalAmount, bool isCancelled, ICollection<SaleItem> items)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        Customer = customer;
        Branch = branch;
        TotalAmount = totalAmount;
        IsCancelled = isCancelled;
        Items = items;
    }
}
